using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.DTOs;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CustomerOnly")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderDetailsController(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository ?? 
                throw new ArgumentNullException(nameof(orderDetailRepository));
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "CustomerOnly")]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderDetailsById(int id)
        {
            try
            {
                var orderDetail = await _orderDetailRepository.GetOrderDetailByIdAsync(id);
                if (orderDetail == null)
                {
                    return NotFound(new {message = $"Order detail with Id {id} not found." });
                }

                var result = new OrderDetailsDTO
                {
                    Id = orderDetail.Id,
                    ProductName = orderDetail.ProductName,
                    ColorName = orderDetail.ColorName,
                    SizeName = orderDetail.SizeName,
                    Price = orderDetail.Price,
                    Quantity = orderDetail.Quantity,
                    OrderId = orderDetail.OrderId,
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new {message = ex.Message});
            }
        }

        [HttpGet("order/{orderId}")]
        [Authorize(Policy = "CustomerOnly")]
        public async Task<ActionResult<ICollection<OrderDetailsDTO>>> GetAllOrderDetailsByOrderId(int orderId)
        {
            try
            {
                var orderDetails = await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(orderId);
                if (orderDetails == null || orderDetails.Count == 0)
                {
                    return NotFound(new { message = $"No order details found for Order Id {orderId}." });
                }

                var result = new List<OrderDetailsDTO>();
                foreach (var item in orderDetails)
                {
                    result.Add(new OrderDetailsDTO
                    {
                        Id = item.Id,
                        ProductName = item.ProductName,
                        ColorName = item.ColorName,
                        SizeName = item.SizeName,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        OrderId = item.OrderId,
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderDetail([FromBody] List<AddOrderDetailsDTO> newOrderDetails)
        {
            if (newOrderDetails == null || !newOrderDetails.Any())
            {
                return BadRequest(new { message = "Order details data cannot be null or empty." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Process the order details using the enhanced AddOrderDetailAsync method
                var failedOrderDetails = await _orderDetailRepository.AddOrderDetailAsync(newOrderDetails);

                // If any order details failed due to stock issues, return the failed items
                if (failedOrderDetails.Any())
                {
                    return BadRequest(new
                    {
                        message = "Some items could not be processed due to insufficient stock.",
                        failedItems = failedOrderDetails
                    });
                }

                return Ok(new { message = "Order details added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetailById(int id)
        {
            try
            {
                await _orderDetailRepository.DeleteOrderDetailByIdAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new {message = $"Order detail with Id {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CustomerOnly")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        // GET: api/Order/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<GetOrderDTO>> GetOrder(int id)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(id);

                var orderDTO = new GetOrderDTO
                {
                    Id = order.Id,
                    InvoiceNumber = order.InvoiceNumber,
                    OpeningDate = order.OpeningDate,
                    ClosingDate = order.ClosingDate,
                    Comments = order.Comments,
                    AddressId = order.AddressId
                };

                return Ok(orderDTO);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<ICollection<GetOrderDTO>>> GetOrders([FromQuery] string userId)
        {
            try
            {
                var orders = await _orderRepository.GetAllOrdersByUserIdAsync(userId);
                if (orders == null)
                {
                    return NotFound(new { message = "This user doesn't have orders" });
                }

                var orderDTOs = orders.Select(o => new GetOrderDTO
                {
                    Id = o.Id,
                    InvoiceNumber = o.InvoiceNumber,
                    OpeningDate = o.OpeningDate,
                    Comments = o.Comments,
                    AddressId = o.AddressId
                }).ToList();

                return Ok(orderDTOs);
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody] AddOrderDTO addOrderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = new Order
            {
                InvoiceNumber = addOrderDTO.InvoiceNumber,
                OpeningDate = addOrderDTO.OpeningDate,
                Comments = addOrderDTO.Comments,
                AddressId = addOrderDTO.AddressId,
                UserId = addOrderDTO.userId
            };

            try
            {
                await _orderRepository.AddOrderAsync(order);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        // PUT: api/Order/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO updateOrderDTO, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
                if (existingOrder.UserId != userId)
                {
                    return NotFound(new {message = "This user isn't the owner of this order"});
                }

                var order = new Order
                {
                    ClosingDate = updateOrderDTO.ClosingDate,
                    Comments = updateOrderDTO.Comments,
                    AddressId = updateOrderDTO.AddressId
                };

                await _orderRepository.UpdateOrderAsync(order, id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        // DELETE: api/Order/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id, string userId)
        {

            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(id);
                if (order.UserId != userId)
                {
                    return NotFound(new { message = "This user isn't the owner of this order" });
                }

                await _orderRepository.DeleteOrderByIdAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
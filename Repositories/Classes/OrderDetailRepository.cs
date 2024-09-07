using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Classes
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ECommerceContext _context;
        private readonly IColorSizeProductRepository _colorSizeProductRepository;
        public OrderDetailRepository(ECommerceContext context, IColorSizeProductRepository colorSizeProductRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _colorSizeProductRepository = colorSizeProductRepository;
        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id)
                ?? throw new KeyNotFoundException($"Order detail with Id {id} not found.");

            return orderDetail;
        }

        public async Task<ICollection<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<List<OrderDetail>> AddOrderDetailAsync(List<AddOrderDetailsDTO> orderDetailsDTOs)
        {
            if (orderDetailsDTOs == null || !orderDetailsDTOs.Any())
            {
                throw new ArgumentNullException(nameof(orderDetailsDTOs), "Order details cannot be null or empty.");
            }

            var failedOrderDetails = new List<OrderDetail>();

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var orderDetailsDTO in orderDetailsDTOs)
                    {
                        // Fetch the corresponding stock for the given product, color, and size
                        var colorSizeProduct = await _colorSizeProductRepository.Get(orderDetailsDTO.ProductId, orderDetailsDTO.ColorId, orderDetailsDTO.SizeId);

                        if (colorSizeProduct == null)
                        {
                            // Create a failed order detail object from the DTO and add to the failed list
                            failedOrderDetails.Add(MapToOrderDetail(orderDetailsDTO));
                            continue;
                        }

                        // Check if requested quantity is greater than available stock
                        if (orderDetailsDTO.Quantity > colorSizeProduct.Quantity)
                        {
                            failedOrderDetails.Add(MapToOrderDetail(orderDetailsDTO));
                            continue;
                        }

                        // Map DTO to OrderDetail and set necessary fields
                        var orderDetail = MapToOrderDetail(orderDetailsDTO);
                        orderDetail.Quantity = orderDetailsDTO.Quantity;
                        orderDetail.Price = orderDetailsDTO.Price;
                        orderDetail.ProductName = orderDetailsDTO.ProductName;
                        orderDetail.ColorName = orderDetailsDTO.ColorName;
                        orderDetail.SizeName = orderDetailsDTO.SizeName;

                        // Decrease stock quantity for the product
                        colorSizeProduct.Quantity -= orderDetailsDTO.Quantity;

                        // Add the valid order detail to the database
                        await _context.OrderDetails.AddAsync(orderDetail);
                    }

                    // Check if there are any failed order details due to stock issues
                    if (failedOrderDetails.Any())
                    {
                        // Rollback the transaction if any order detail could not be processed
                        await transaction.RollbackAsync();

                        // Return the failed order details for further handling
                        return failedOrderDetails;
                    }

                    // Save changes and commit the transaction if all order details are valid
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (DbUpdateException dbEx)
                {
                    // Rollback the transaction in case of a database update failure
                    await transaction.RollbackAsync();
                    throw new Exception("An error occurred while adding the order details to the database.", dbEx);
                }
                catch (Exception ex)
                {
                    // Rollback the transaction in case of a general failure
                    await transaction.RollbackAsync();
                    throw new Exception("An unexpected error occurred while adding the order details.", ex);
                }
            }

            // Return an empty list if all order details were successfully processed
            return failedOrderDetails;
        }
        private OrderDetail MapToOrderDetail(AddOrderDetailsDTO dto)
        {
            return new OrderDetail
            {
                ProductName = dto.ProductName,
                ColorName = dto.ColorName,
                SizeName = dto.SizeName,
                Price = dto.Price,
                Quantity = dto.Quantity,
                OrderId = dto.OrderId
            };
        }



        public async Task DeleteOrderDetailByIdAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                throw new KeyNotFoundException($"Order detail with Id {id} not found.");
            }

            try
            {
                _context.OrderDetails.Remove(orderDetail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while deleting the order detail from the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the order detail.", ex);
            }
        }
    }
}
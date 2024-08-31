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

        public OrderDetailRepository(ECommerceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<OrderDetail>> GetAllOrderDetailsAsync()
        {
            return await _context.OrderDetails.ToListAsync();
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

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            if (orderDetail == null)
            {
                throw new ArgumentNullException(nameof(orderDetail), "Order detail cannot be null.");
            }

            try
            {
                await _context.OrderDetails.AddAsync(orderDetail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while adding the order detail to the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the order detail.", ex);
            }
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
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Classes
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ECommerceContext _context;

        public OrderRepository(ECommerceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id)
                ?? throw new KeyNotFoundException($"Order with Id {id} not found.");

            return order;
        }

        public async Task AddOrderAsync(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }

            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while adding the order to the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the order.", ex);
            }
        }

        public async Task DeleteOrderByIdAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with Id {id} not found.");
            }

            try
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while deleting the order from the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the order.", ex);
            }
        }

        public async Task UpdateOrderAsync(Order order, int id)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }

            var existingOrder = await _context.Orders.FindAsync(id)
                ?? throw new KeyNotFoundException($"Order with Id {id} not found.");

            existingOrder.ClosingDate = order.ClosingDate;
            existingOrder.Comments = order.Comments;
            existingOrder.AddressId = order.AddressId;
            existingOrder.Status = order.Status;

            try
            {
                _context.Orders.Update(existingOrder);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                throw new Exception("A concurrency error occurred while updating the order.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the order.", ex);
            }
        }

        public async Task<ICollection<Order>> GetAllOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders.Where(o => o.UserId ==  userId).ToListAsync();
        }
    }
}
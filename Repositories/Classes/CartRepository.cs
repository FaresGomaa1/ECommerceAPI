using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Classes
{
    public class CartRepository : ICartRepository
    {
        private readonly ECommerceContext _context;

        public CartRepository(ECommerceContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Cart>> GetAllCartsAsync(string userId)
        {
            return await _context.Carts
                                 .Where(c => c.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<Cart> GetCartByIdAsync(int id)
        {
            return await _context.Carts.FindAsync(id) ?? throw new KeyNotFoundException($"Cart with Id {id} not found.");
        }

        public async Task AddCartAsync(Cart cart)
        {
            try
            {
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the cart.", ex);
            }
        }

        public async Task DeleteCartByIdAsync(int id)
        {
            var cart = await GetCartByIdAsync(id);
            if (cart != null) 
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Cart with Id {id} not found.");
            }

        }

        public async Task UpdateCartAsync(Cart cart, int id)
        {
            var existingCart = await GetCartByIdAsync(id);

            existingCart.Price = cart.Price;
            existingCart.Quantity = cart.Quantity;

            _context.Carts.Update(existingCart);
            await _context.SaveChangesAsync();
        }
    }
}
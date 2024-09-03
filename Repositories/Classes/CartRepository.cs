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
        public async Task<Cart> GetCartByUserProductColorSizeAsync(string userId, int productId, int colorId, int sizeId)
        {
            return await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId
                                       && c.ProductId == productId
                                       && c.ColorId == colorId
                                       && c.SizeId == sizeId);
        }


        public async Task AddCartAsync(Cart cart)
        {
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
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
        public async Task DeleteAllUserItemsAsync(string userId)
        {
            var userCarts = _context.Carts.Where(cart => cart.UserId == userId);

            if (userCarts.Any())
            {
                _context.Carts.RemoveRange(userCarts);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"No cart items found for user with Id {userId}.");
            }
        }

        public async Task<bool> UpdateCartAsync(int quantity, int id)
        {
            var existingCart = await GetCartByIdAsync(id);

            if (existingCart == null)
            {
                return false;
            }

            existingCart.Quantity = quantity;

            _context.Carts.Update(existingCart);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
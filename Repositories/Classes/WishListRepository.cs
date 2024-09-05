using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Classes
{
    public class WishListRepository : IWishListRepository
    {
        private readonly ECommerceContext _context;

        public WishListRepository(ECommerceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<WishList>> GetAllWishListsAsync(string userId)
        {
            return await _context.WishLists
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<WishList> GetWishListByIdAsync(int id)
        {
            var wishList = await _context.WishLists.FindAsync(id)
                ?? throw new KeyNotFoundException($"WishList with Id {id} not found.");

            return wishList;
        }

        public async Task AddWishListAsync(WishList wishList)
        {
            if (wishList == null)
            {
                throw new ArgumentNullException(nameof(wishList), "WishList cannot be null.");
            }

            try
            {
                await _context.WishLists.AddAsync(wishList);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while adding the wishlist to the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the wishlist.", ex);
            }
        }

        public async Task DeleteWishListByIdAsync(int id)
        {
            var wishList = await _context.WishLists.FindAsync(id);
            if (wishList == null)
            {
                throw new KeyNotFoundException($"WishList with Id {id} not found.");
            }

            try
            {
                _context.WishLists.Remove(wishList);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while deleting the wishlist from the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the wishlist.", ex);
            }
        }
        public async Task<WishList> GetWishListItemByUserIdAndProductId(string userId, int productId)
        {
            return await _context.WishLists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);
        }

    }
}
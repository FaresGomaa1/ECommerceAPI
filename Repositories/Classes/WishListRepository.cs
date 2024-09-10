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

            // Check if the combination of UserId and ProductId already exists
            var existingWishListItem = await _context.WishLists
                .FirstOrDefaultAsync(w => w.UserId == wishList.UserId && w.ProductId == wishList.ProductId);

            if (existingWishListItem != null)
            {
                // Throw InvalidOperationException if the item already exists
                throw new InvalidOperationException("This product is already in the user's wishlist.");
            }

            // If the combination does not exist, add the new wishlist item
            _context.WishLists.Add(wishList);
            await _context.SaveChangesAsync();
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
        public async Task<WishList> GetWishListItemByUserIdAndProductIdAsync(string userId, int productId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
            }

            var wishListItem = await _context.WishLists
                .SingleOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (wishListItem == null)
            {
                // Optionally, you could throw an exception or handle this case differently
                throw new KeyNotFoundException($"No wishlist item found for UserId: {userId} and ProductId: {productId}");
            }

            return wishListItem;
        }
    }
}
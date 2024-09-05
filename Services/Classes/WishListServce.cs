using ECommerceAPI.DTOs;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;
using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceAPI.Services.Classes
{
    public class WishListService : IWishListService
    {
        private readonly IWishListRepository _wishListRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IRateRepository _rateRepository;

        public WishListService(IWishListRepository wishListRepository, ICategoryRepository categoryRepository,
            IProductRepository productRepository, IRateRepository rateRepository)
        {
            _wishListRepository = wishListRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _rateRepository = rateRepository;
        }

        // Add item to wishlist with proper error handling
        public async Task AddItemToWishList(AddItemToWishList item)
        {
            // Check if the item already exists in the wishlist
            var existingItem = await _wishListRepository.GetWishListItemByUserIdAndProductId(item.UserId, item.ProductId);
            if (existingItem != null)
            {
                throw new InvalidOperationException("Item already exists in the wishlist.");
            }

            // Add new item to wishlist
            var newItem = new WishList
            {
                UserId = item.UserId,
                ProductId = item.ProductId,
            };

            await _wishListRepository.AddWishListAsync(newItem);
        }

        // Delete a wishlist item with validation
        public async Task DeleteWishListItem(int itemId, string userId)
        {
            var wishListItem = await _wishListRepository.GetWishListByIdAsync(itemId);

            if (wishListItem == null || wishListItem.UserId != userId)
            {
                throw new KeyNotFoundException($"WishList item with ID {itemId} not found or doesn't belong to the user.");
            }

            await _wishListRepository.DeleteWishListByIdAsync(itemId);
        }

        // Get user's wishlist with related product and rate data
        public async Task<ICollection<WishListDTO>> GetUserWishList(string userId)
        {
            var wishLists = await _wishListRepository.GetAllWishListsAsync(userId);

            var result = await Task.WhenAll(wishLists.Select(async item =>
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                var category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);
                var rates = await _rateRepository.GetRateByProductIdAsync(item.ProductId);

                var averageRating = rates.Any() ? rates.Average(r => r.Value) : 0;

                return new WishListDTO
                {
                    Id = item.Id,
                    CategoryName = category.Name,
                    ProductName = product.Name,
                    ProductId = product.Id,
                    Price = product.Price,
                    Rate = (decimal)averageRating
                };
            }));

            return result.ToList();
        }
    }
}
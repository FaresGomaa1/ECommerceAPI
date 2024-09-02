using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceAPI.Services.Classes
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<ICollection<GetCartDTO>> GetAllCartItems(string userId)
        {
            // Fetch all cart items for the user
            var cartItems = await _cartRepository.GetAllCartsAsync(userId);

            // Use LINQ to map Cart entities to GetCartDTOs
            var result = await Task.WhenAll(cartItems.Select(async item =>
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                return new GetCartDTO
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    Quantity = item.Quantity,
                };
            }));

            return result.ToList();
        }
    }
}
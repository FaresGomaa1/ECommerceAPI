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
        private readonly IColorRepository _colorRepository;
        private readonly ISizeRepository _sizeRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository, IColorRepository colorRepository, ISizeRepository sizeRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;   
        }

        public async Task AddItemAsync(AddCart item)
        {
            // Check if the item already exists in the cart for the user
            var existingCartItem = await _cartRepository.GetCartByUserProductColorSizeAsync(
                item.UserId, item.ProductId, item.ColorId, item.SizeId);

            if (existingCartItem != null)
            {
                throw new InvalidOperationException("This item already exists in your cart with the same product, color, and size.");
            }

            // If it doesn't exist, add the new item to the cart
            Cart newCart = new Cart()
            {
                ProductId = item.ProductId,
                SizeId = item.SizeId,
                ColorId = item.ColorId,
                Quantity = item.Quantity,
                UserId = item.UserId,
            };

            await _cartRepository.AddCartAsync(newCart);
        }


        public async Task<ICollection<GetCartDTO>> GetAllCartItems(string userId)
        {
            var cartItems = await _cartRepository.GetAllCartsAsync(userId);
            var result = new List<GetCartDTO>();

            foreach (var item in cartItems)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                var color = await _colorRepository.GetColorByIdAsync(item.ColorId);
                var size = await _sizeRepository.GetSizeByIdAsync(item.SizeId);

                result.Add(new GetCartDTO
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    Quantity = item.Quantity,
                    Color = color.Name,
                    ColorId = item.ColorId,
                    Size = size.Name,
                    SizeId = item.SizeId,
                });
            }

            return result;
        }


        public async Task<bool> UpdateQuantityAsync(int quantity, int itemId)
        {
            return await _cartRepository.UpdateCartAsync(quantity, itemId);
        }
        public async Task DeleteItem(int itemId)
        {
            await _cartRepository.DeleteCartByIdAsync(itemId);
        }
        public async Task DeleteAllUserItemInCart(string userId)
        {
            await _cartRepository.DeleteAllUserItemsAsync(userId);
        }

    }
}
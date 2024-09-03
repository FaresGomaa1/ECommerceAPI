using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<ICollection<Cart>> GetAllCartsAsync(string userId);
        Task AddCartAsync(Cart cart);
        Task DeleteCartByIdAsync(int id);
        Task<bool> UpdateCartAsync(int quantity, int id);
        Task DeleteAllUserItemsAsync(string userId);
        Task<Cart> GetCartByUserProductColorSizeAsync(string userId, int productId, int colorId, int sizeId);
    }
}

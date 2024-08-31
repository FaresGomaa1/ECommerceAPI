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
        Task UpdateCartAsync(Cart cart, int id);
    }
}

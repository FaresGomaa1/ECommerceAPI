using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<ICollection<Order>> GetAllOrdersByUserIdAsync(string userId);
        Task<Order> GetOrderByIdAsync(int id);
        Task AddOrderAsync(Order order);
        Task DeleteOrderByIdAsync(int id);
        Task UpdateOrderAsync(Order order, int id);

    }
}

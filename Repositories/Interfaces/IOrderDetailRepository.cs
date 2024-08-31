using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IOrderDetailRepository
    {
        Task<ICollection<OrderDetail>> GetAllOrderDetailsAsync();
        Task<OrderDetail> GetOrderDetailByIdAsync(int id);
        Task<ICollection<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task AddOrderDetailAsync(OrderDetail orderDetail);
        Task DeleteOrderDetailByIdAsync(int id);
    }
}

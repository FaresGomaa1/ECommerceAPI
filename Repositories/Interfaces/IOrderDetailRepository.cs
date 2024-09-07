using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IOrderDetailRepository
    {
        Task<OrderDetail> GetOrderDetailByIdAsync(int id);
        Task<ICollection<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task<List<OrderDetail>> AddOrderDetailAsync(List<AddOrderDetailsDTO> orderDetailsDTOs);
        Task DeleteOrderDetailByIdAsync(int id);
    }
}

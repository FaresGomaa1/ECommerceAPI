using ECommerceAPI.DTOs;

namespace ECommerceAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<GetAllProductDTO>> GetAllProducts();
    }
}

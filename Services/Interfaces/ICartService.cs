using ECommerceAPI.DTOs;
namespace ECommerceAPI.Services.Interfaces
{
    public interface ICartService
    {
        Task<ICollection<GetCartDTO>> GetAllCartItems(string userId);
    }
}

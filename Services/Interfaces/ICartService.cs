using ECommerceAPI.DTOs;
namespace ECommerceAPI.Services.Interfaces
{
    public interface ICartService
    {
        Task<ICollection<GetCartDTO>> GetAllCartItems(string userId);
        Task AddItemAsync(AddCart item);
        Task<bool> UpdateQuantityAsync(int quantity, int itemId);
        Task DeleteAllUserItemInCart(string userId);
        Task DeleteItem(int itemId);
    }
}

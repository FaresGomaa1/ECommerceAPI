using ECommerceAPI.DTOs;

namespace ECommerceAPI.Services.Interfaces
{
    public interface IWishListService
    {
        Task<ICollection<WishListDTO>> GetUserWishList(string userId);
        Task DeleteWishListItem(int itemId, string userId);
        Task AddItemToWishList(AddItemToWishList item);
    }
}

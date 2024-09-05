using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IWishListRepository
    {
        Task<ICollection<WishList>> GetAllWishListsAsync(string userId);
        Task<WishList> GetWishListByIdAsync(int id);
        Task AddWishListAsync(WishList wishList);
        Task DeleteWishListByIdAsync(int id);
        Task<WishList> GetWishListItemByUserIdAndProductId(string userId, int productId);
    }
}

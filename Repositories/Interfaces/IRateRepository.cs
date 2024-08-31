using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IRateRepository
    {
        Task<ICollection<Rate>> GetAllRatesAsync();
        Task<Rate> GetRateByIdAsync(int id);
        Task AddRateAsync(Rate rate);
        Task DeleteRateByIdAsync(int id);
        Task UpdateRateAsync(Rate rate, int id);
        Task<ICollection<Rate>> GetRateByProductIdAsync(int productId);
        Task<bool> IsRateExistAsync(int productId, string userId);
    }
}

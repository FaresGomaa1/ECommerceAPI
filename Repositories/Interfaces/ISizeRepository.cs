using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface ISizeRepository
    {
        Task<ICollection<Size>> GetAllSizesAsync();
        Task<Size> GetSizeByIdAsync(int id);
        Task AddSizeAsync(Size size);
        Task DeleteSizeByIdAsync(int id);
        Task UpdateSizeAsync(Size size, int id);
    }
}

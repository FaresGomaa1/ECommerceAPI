using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IColorRepository
    {
        Task<ICollection<Color>> GetAllColorsAsync();
        Task<Color> GetColorByIdAsync(int id);
        Task AddColorAsync(Color color);
        Task DeleteColorByIdAsync(int id);
        Task UpdateColorAsync(Color color, int id);
    }
}

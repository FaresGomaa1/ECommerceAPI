using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IPhotoRepository
    {
        Task<ICollection<Photo>> GetAllPhotosAsync();
        Task<ICollection<Photo>> GetPhotosByProductIdAsync(int productId);
        Task<Photo> GetPhotoByIdAsync(int id);
        Task AddPhotoAsync(Photo photo);
        Task DeletePhotoByIdAsync(int id);
        Task UpdatePhotoAsync(Photo photo, int id);
    }
}

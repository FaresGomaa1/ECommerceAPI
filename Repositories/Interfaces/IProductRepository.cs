using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<ICollection<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<ICollection<Product>> GetProductsByCategoryAsync(int categoryId);
        Task AddProductAsync(Product product);
        Task DeleteProductByIdAsync(int id);
        Task UpdateProductAsync(Product product, int id);
    }
}

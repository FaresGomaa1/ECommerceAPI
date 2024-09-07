using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IColorSizeProductRepository
    {
        Task<ICollection<ColorSizeProduct>> GetAllProductSizesAndColorsAsync(int productId);
        Task<ColorSizeProduct> GetProductColorSizeByIdAsync(int id);
        Task AddProductColorSizeAsync(ColorSizeProduct colorSizeProduct);
        Task DeleteProductColorSizeAsync(int id);
        Task UpdateProductColorSizeAsync(ColorSizeProduct colorSizeProduct, int id);
        Task<ColorSizeProduct> Get(int productId, int colorId, int sizeId);
    }
}

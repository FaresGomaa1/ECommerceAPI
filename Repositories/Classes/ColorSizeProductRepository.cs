using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Classes
{
    public class ColorSizeProductRepository : IColorSizeProductRepository
    {
        private readonly ECommerceContext _context;

        public ColorSizeProductRepository(ECommerceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<ColorSizeProduct>> GetAllProductSizesAndColorsAsync(int productId)
        {
            try
            {
                return await _context.ColorSizeProducts
                    .Where(csp => csp.ProductId == productId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving product sizes and colors.", ex);
            }
        }

        public async Task AddProductColorSizeAsync(ColorSizeProduct colorSizeProduct)
        {
            if (colorSizeProduct == null)
            {
                throw new ArgumentNullException(nameof(colorSizeProduct), "ColorSizeProduct cannot be null.");
            }

            try
            {
                await _context.ColorSizeProducts.AddAsync(colorSizeProduct);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while adding the product color and size to the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the product color and size.", ex);
            }
        }

        public async Task DeleteProductColorSizeAsync(int id)
        {
            var colorSizeProduct = await _context.ColorSizeProducts.FindAsync(id);
            if (colorSizeProduct == null)
            {
                throw new KeyNotFoundException($"ColorSizeProduct with Id {id} not found.");
            }

            try
            {
                _context.ColorSizeProducts.Remove(colorSizeProduct);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while deleting the product color and size from the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the product color and size.", ex);
            }
        }

        public async Task UpdateProductColorSizeAsync(ColorSizeProduct colorSizeProduct, int id)
        {
            if (colorSizeProduct == null)
            {
                throw new ArgumentNullException(nameof(colorSizeProduct), "ColorSizeProduct cannot be null.");
            }

            var existingColorSizeProduct = await _context.ColorSizeProducts.FindAsync(id)
                ?? throw new KeyNotFoundException($"ColorSizeProduct with Id {id} not found.");

            existingColorSizeProduct.ColorId = colorSizeProduct.ColorId;
            existingColorSizeProduct.SizeId = colorSizeProduct.SizeId;
            existingColorSizeProduct.Quantity = colorSizeProduct.Quantity;
            existingColorSizeProduct.Price = colorSizeProduct.Price;
            try
            {
                _context.ColorSizeProducts.Update(existingColorSizeProduct);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                throw new Exception("A concurrency error occurred while updating the ColorSizeProduct.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the ColorSizeProduct.", ex);
            }

        }

        public async Task<ColorSizeProduct> GetProductColorSizeByIdAsync(int id)
        {
            var colorSizeProduct = await _context.ColorSizeProducts.FindAsync(id);

            if (colorSizeProduct == null)
            {
                throw new KeyNotFoundException($"Product color and size with Id {id} not found.");
            }

            return colorSizeProduct;
        }

    }
}

using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Classes
{
    public class ColorRepository : IColorRepository
    {
        private readonly ECommerceContext _context;

        public ColorRepository(ECommerceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<Color>> GetAllColorsAsync()
        {
            try
            {
                return await _context.Colors.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the colors.", ex);
            }
        }

        public async Task<Color> GetColorByIdAsync(int id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color == null)
            {
                throw new KeyNotFoundException($"Color with Id {id} not found.");
            }

            return color;
        }

        public async Task AddColorAsync(Color color)
        {
            if (color == null)
            {
                throw new ArgumentNullException(nameof(color), "Color cannot be null.");
            }

            try
            {
                await _context.Colors.AddAsync(color);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while adding the color to the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the color.", ex);
            }
        }

        public async Task DeleteColorByIdAsync(int id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color == null)
            {
                throw new KeyNotFoundException($"Color with Id {id} not found.");
            }

            try
            {
                _context.Colors.Remove(color);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while deleting the color from the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the color.", ex);
            }
        }

        public async Task UpdateColorAsync(Color color, int id)
        {
            if (color == null)
            {
                throw new ArgumentNullException(nameof(color), "Color cannot be null.");
            }

            var existingColor = await _context.Colors.FindAsync(id)
                ?? throw new KeyNotFoundException($"Color with Id {id} not found.");

            existingColor.Name = color.Name;

            try
            {
                _context.Colors.Update(existingColor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                throw new Exception("A concurrency error occurred while updating the color.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the color.", ex);
            }
        }
    }
}
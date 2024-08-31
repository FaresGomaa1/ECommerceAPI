using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Classes
{
    public class SizeRepository : ISizeRepository
    {
        private readonly ECommerceContext _context;

        public SizeRepository(ECommerceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<Size>> GetAllSizesAsync()
        {
            return await _context.Sizes.ToListAsync();
        }

        public async Task<Size> GetSizeByIdAsync(int id)
        {
            var size = await _context.Sizes.FindAsync(id)
                ?? throw new KeyNotFoundException($"Size with Id {id} not found.");

            return size;
        }

        public async Task AddSizeAsync(Size size)
        {
            if (size == null)
            {
                throw new ArgumentNullException(nameof(size), "Size cannot be null.");
            }

            try
            {
                await _context.Sizes.AddAsync(size);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while adding the size to the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the size.", ex);
            }
        }

        public async Task DeleteSizeByIdAsync(int id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                throw new KeyNotFoundException($"Size with Id {id} not found.");
            }

            try
            {
                _context.Sizes.Remove(size);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while deleting the size from the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the size.", ex);
            }
        }

        public async Task UpdateSizeAsync(Size size, int id)
        {
            if (size == null)
            {
                throw new ArgumentNullException(nameof(size), "Size cannot be null.");
            }

            var existingSize = await _context.Sizes.FindAsync(id)
                ?? throw new KeyNotFoundException($"Size with Id {id} not found.");

            existingSize.Name = size.Name;

            try
            {
                _context.Sizes.Update(existingSize);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                throw new Exception("A concurrency error occurred while updating the size.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the size.", ex);
            }
        }
    }
}
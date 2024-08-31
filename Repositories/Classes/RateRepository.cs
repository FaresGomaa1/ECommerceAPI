using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Classes
{
    public class RateRepository : IRateRepository
    {
        private readonly ECommerceContext _context;

        public RateRepository(ECommerceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<Rate>> GetAllRatesAsync()
        {
            return await _context.Rates.ToListAsync();
        }

        public async Task<Rate> GetRateByIdAsync(int id)
        {
            var rate = await _context.Rates.FindAsync(id)
                ?? throw new KeyNotFoundException($"Rate with Id {id} not found.");

            return rate;
        }

        public async Task AddRateAsync(Rate rate)
        {
            if (rate == null)
            {
                throw new ArgumentNullException(nameof(rate), "Rate cannot be null.");
            }

            try
            {
                await _context.Rates.AddAsync(rate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while adding the rate to the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the rate.", ex);
            }
        }

        public async Task DeleteRateByIdAsync(int id)
        {
            var rate = await _context.Rates.FindAsync(id);
            if (rate == null)
            {
                throw new KeyNotFoundException($"Rate with Id {id} not found.");
            }

            try
            {
                _context.Rates.Remove(rate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while deleting the rate from the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the rate.", ex);
            }
        }

        public async Task UpdateRateAsync(Rate rate, int id)
        {
            if (rate == null)
            {
                throw new ArgumentNullException(nameof(rate), "Rate cannot be null.");
            }

            var existingRate = await _context.Rates.FindAsync(id)
                ?? throw new KeyNotFoundException($"Rate with Id {id} not found.");

            existingRate.Value = rate.Value;
            try
            {
                _context.Rates.Update(existingRate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                throw new Exception("A concurrency error occurred while updating the rate.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the rate.", ex);
            }
        }

        public async Task<ICollection<Rate>> GetRateByProductIdAsync(int productId)
        {
            return await _context.Rates.Where(r => r.ProductId == productId).ToListAsync();
        }
        public async Task<bool> IsRateExistAsync(int productId, string userId)
        {
            return await _context.Rates
                .AnyAsync(r => r.UserId == userId && r.ProductId == productId);
        }

    }
}
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Classes
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ECommerceContext _context;

        public AddressRepository(ECommerceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAddressAsync(Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address), "Address cannot be null.");
            }

            try
            {
                await _context.Addresses.AddAsync(address);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while adding the address to the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the address.", ex);
            }
        }

        public async Task DeleteAddressByIdAsync(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                throw new KeyNotFoundException($"Address with Id {id} not found.");
            }

            try
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while deleting the address from the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the address.", ex);
            }
        }

        public async Task<ICollection<Address>> GetAllAddressAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));
            }

            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task UpdateAddressAsync(Address address, int id)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address), "Address cannot be null.");
            }

            var existingAddress = await _context.Addresses.FindAsync(id)
                ?? throw new KeyNotFoundException($"Address with Id {id} not found.");

            existingAddress.City = address.City;
            existingAddress.Country = address.Country;
            existingAddress.AddressLine = address.AddressLine;
            existingAddress.State = address.State;

            try
            {
                _context.Addresses.Update(existingAddress);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                throw new Exception("A concurrency error occurred while updating the address.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the address.", ex);
            }
        }
    }
}

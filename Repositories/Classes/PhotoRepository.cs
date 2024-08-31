using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Classes
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly ECommerceContext _context;

        public PhotoRepository(ECommerceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<Photo>> GetAllPhotosAsync()
        {
            return await _context.Photos.ToListAsync();
        }

        public async Task<Photo> GetPhotoByIdAsync(int id)
        {
            var photo = await _context.Photos.FindAsync(id)
                ?? throw new KeyNotFoundException($"Photo with Id {id} not found.");

            return photo;
        }

        public async Task AddPhotoAsync(Photo photo)
        {
            if (photo == null)
            {
                throw new ArgumentNullException(nameof(photo), "Photo cannot be null.");
            }

            try
            {
                await _context.Photos.AddAsync(photo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while adding the photo to the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the photo.", ex);
            }
        }

        public async Task DeletePhotoByIdAsync(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
            {
                throw new KeyNotFoundException($"Photo with Id {id} not found.");
            }

            try
            {
                _context.Photos.Remove(photo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("An error occurred while deleting the photo from the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the photo.", ex);
            }
        }

        public async Task UpdatePhotoAsync(Photo photo, int id)
        {
            if (photo == null)
            {
                throw new ArgumentNullException(nameof(photo), "Photo cannot be null.");
            }

            var existingPhoto = await _context.Photos.FindAsync(id)
                ?? throw new KeyNotFoundException($"Photo with Id {id} not found.");

            // Update fields as necessary
            existingPhoto.Url = photo.Url;
            photo.ProductId = photo.ProductId;
            // Add any other fields that need to be updated

            try
            {
                _context.Photos.Update(existingPhoto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                throw new Exception("A concurrency error occurred while updating the photo.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the photo.", ex);
            }
        }

        public async Task<ICollection<Photo>> GetPhotosByProductIdAsync(int productId)
        {
            return await _context.Photos.Where(p => p.ProductId == productId).ToListAsync();
        }
    }
}
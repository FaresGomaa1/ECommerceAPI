using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;

        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        // GET: api/Address?userId={userId}
        [HttpGet]
        public async Task<ActionResult<ICollection<GetAddressDTO>>> GetAllUserAddresses(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new { message = "User ID is required." });
            }

            var addresses = await _addressRepository.GetAllAddressAsync(userId);

            if (addresses == null || !addresses.Any())
            {
                return NotFound(new { message = "No addresses found for this user." });
            }

            var addressDTOs = addresses.Select(item => new GetAddressDTO
            {
                Id = item.Id,
                Country = item.Country,
                AddressLine = item.AddressLine,
                City = item.City,
                State = item.State
            }).ToList();

            return Ok(addressDTOs);
        }

        // POST: api/Address
        [HttpPost]
        public async Task<IActionResult> AddNewAddress(string userId, [FromBody] AddAddressDTO newAddress)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(ModelState);
            }

            var address = new Address
            {
                UserId = userId,
                AddressLine = newAddress.AddressLine,
                City = newAddress.City,
                State = newAddress.State,
                Country = newAddress.Country
            };

            await _addressRepository.AddAddressAsync(address);

            var createdAddress = new GetAddressDTO
            {
                Id = address.Id,
                Country = address.Country,
                AddressLine = address.AddressLine,
                City = address.City,
                State = address.State
            };

            return CreatedAtAction(nameof(GetAllUserAddresses), new { userId = userId }, createdAddress);
        }

        // PUT: api/Address/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, string userId, [FromBody] GetAddressDTO updatedAddress)
        {
            if (!ModelState.IsValid || id <= 0 || string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new { message = "Invalid input data." });
            }

            try
            {
                // Update address via the repository method
                var addressToUpdate = new Address
                {
                    AddressLine = updatedAddress.AddressLine,
                    City = updatedAddress.City,
                    State = updatedAddress.State,
                    Country = updatedAddress.Country,
                    UserId = userId
                };

                await _addressRepository.UpdateAddressAsync(addressToUpdate, id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the address.", details = ex.Message });
            }
        }

        // DELETE: api/Address/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id, string userId)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new { message = "Invalid address ID or user ID." });
            }

            try
            {
                // Delete address via the repository method
                await _addressRepository.DeleteAddressByIdAsync(id, userId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the address.", details = ex.Message });
            }
        }
    }
}
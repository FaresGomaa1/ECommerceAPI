using ECommerceAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Authorize(Policy = "CustomerOnly")]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;

        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        // GET: api/WishList?userId={userId}
        [HttpGet]
        public async Task<ActionResult<ICollection<WishListDTO>>> GetUserWishList(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new {message = "User ID is required." });
            }

            try
            {
                var wishList = await _wishListService.GetUserWishList(userId);

                if (wishList == null || wishList.Count == 0)
                {
                    return NotFound(new {message = "No wishlist items found for this user." });
                }

                return Ok(wishList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {message = ex.Message});
            }
        }

        // DELETE: api/WishList/{itemId}?userId={userId}
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteWishListItem(int itemId, string userId)
        {
            if (itemId <= 0)
            {
                return BadRequest(new {message = "Invalid item ID." });
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new {message = "User ID is required." });
            }

            try
            {
                await _wishListService.DeleteWishListItem(itemId, userId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/WishList
        [HttpPost]
        public async Task<IActionResult> AddItemToWishList([FromBody] AddItemToWishList newItem)
        {
            if (newItem == null || string.IsNullOrWhiteSpace(newItem.UserId) || newItem.ProductId <= 0)
            {
                return BadRequest(new { message = "Invalid input. Ensure both User ID and Product ID are provided." });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Call the service to add the item to the wishlist
                await _wishListService.AddItemToWishList(newItem);

                // Return a response indicating the item was successfully added
                return CreatedAtAction(nameof(GetUserWishList), new { userId = newItem.UserId }, newItem);
            }
            catch (InvalidOperationException ex)
            {
                // Handle duplicate entry case (item already exists in the wishlist)
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle other unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        [HttpGet("check-exists")]
        public async Task<IActionResult> CheckItemExistInCustomerWishList(int productId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID cannot be null or empty.");
            }

            bool itemExists = await _wishListService.CheckItemExistInCustomerWishList(productId, userId);

            return Ok(itemExists);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteWishListItemByUserIdProductId([FromQuery] int productId, [FromQuery] string userId)
        {
            if (productId <= 0)
            {
                return BadRequest(new { message = "Invalid product ID." });
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new { message = "User ID is required." });
            }

            try
            {
                await _wishListService.DeleteByUserIdProductIdAsync(productId, userId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
using ECommerceAPI.DTOs;
using ECommerceAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "CustomerOnly")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<GetCartDTO>>> GetAllCartItems(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest(new { message = "UserId cannot be null or empty." });
                }

                var cart = await _cartService.GetAllCartItems(userId);
                if (cart == null || cart.Count == 0)
                {
                    return NotFound(new { message = "No cart items found for the user." });
                }

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message});
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCartItem(AddCart item)
        {
            if (item == null)
            {
                return BadRequest(new { message = "Item cannot be null." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = ModelState });
            }

            try
            {
                await _cartService.AddItemAsync(item);
                return Ok(new { message = "Item added to cart successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateItemQuantity(int quantity, int itemId)
        {
            if (quantity <= 0)
            {
                return BadRequest(new { message = "Quantity must be greater than zero." });
            }

            try
            {
                var result = await _cartService.UpdateQuantityAsync(quantity, itemId);

                if (!result)
                {
                    return NotFound(new { message = "Cart item not found." });
                }

                return Ok(new { message = "Item quantity updated successfully." });
            }
            catch (NotSupportedException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            try
            {
                await _cartService.DeleteItem(itemId);
                return Ok(new { message = $"Item with Id {itemId} deleted successfully." });
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

        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteUserItem(string userId)
        {
            try
            {
                await _cartService.DeleteAllUserItemInCart(userId);
                return Ok(new { message = $"All items for user with Id {userId} deleted successfully." });
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
    }
}
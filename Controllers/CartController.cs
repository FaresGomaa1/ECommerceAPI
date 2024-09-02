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
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        [Authorize("Customer")]
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
                return StatusCode(500, ex.Message);
            }
        }
    }
}
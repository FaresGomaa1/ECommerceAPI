using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IRateRepository _rateRepository;

        public RateController(IRateRepository rateRepository)
        {
            _rateRepository = rateRepository;
        }
        [HttpGet("{productId}/user/{userId}")]
        public async Task<ActionResult<RateResponseDTO>> GetRate(int productId, string userId)
        {
            var rateExists = await _rateRepository.IsRateExistAsync(productId, userId);
            if (!rateExists)
            {
                return NotFound(new {message = "Rate not found for the given product and user." });
            }
            var userRate = await _rateRepository.GetRateByProductIdUserIdAsync(productId, userId);

            var response = new RateResponseDTO
            {
                RateId = userRate.Id,
                ProductId = userRate.ProductId,
                UserId = userRate.UserId,
                Value = userRate.Value
            };

            return Ok(response);
        }
        [Authorize]
        [Authorize(Policy = "CustomerOnly")]
        [HttpPost]
        public async Task<IActionResult> AddRate([FromBody] RateDTO rateDto)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the rate already exists
            bool rateExists = await _rateRepository.IsRateExistAsync(rateDto.ProductId, rateDto.UserId);

            if (rateExists)
            {
                return Conflict(new { message = "Rate already exists." });
            }

            // Create new rate
            var newRate = new Rate
            {
                ProductId = rateDto.ProductId,
                UserId = rateDto.UserId,
                Value = rateDto.Value
            };

            // Add the new rate to the repository
            await _rateRepository.AddRateAsync(newRate);

            return Ok(new { message = "Rate added successfully." });
        }
        [Authorize]
        [Authorize(Policy = "CustomerOnly")]
        [HttpPatch]
        public async Task<IActionResult> UpdateRateValue([FromBody] RateDTO rateDto)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the rate exists
            var existingRate = await _rateRepository.GetRateByProductIdUserIdAsync(rateDto.ProductId, rateDto.UserId);
            if (existingRate == null)
            {
                return NotFound(new { message = "Rate not found." });
            }

            // Update the value of the existing rate
            existingRate.Value = rateDto.Value;

            // Update rate in the repository
            await _rateRepository.UpdateRateAsync(existingRate, existingRate.Id);

            return Ok(new { message = "Rate updated successfully." });
        }


    }
}

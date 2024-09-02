using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        // GET: api/category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCategory>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetAllCategoriesAsync();

                if (categories == null || !categories.Any())
                {
                    return NotFound(new { message = "No categories found." });
                }

                var result = categories.Select(c => new GetCategory
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception here (ex) using a logging framework
                return StatusCode(500, new { message = "An error occurred while retrieving the categories.", details = ex.Message });
            }
        }

        // GET: api/category/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCategory>> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(id);

                if (category == null)
                {
                    return NotFound(new { message = $"Category with Id {id} not found." });
                }

                var result = new GetCategory
                {
                    Id = category.Id,
                    Name = category.Name
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception here (ex) using a logging framework
                return StatusCode(500, new {details = ex.Message });
            }
        }

        // POST: api/category
        [HttpPost]
        public async Task<ActionResult<GetCategory>> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (createCategoryDto == null)
            {
                return BadRequest(new { message = "Category data is null." });
            }

            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid data.", errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var category = new Category
                {
                    Name = createCategoryDto.Name
                };

                await _categoryRepository.AddCategoryAsync(category);

                var createdCategory = new GetCategory
                {
                    Id = category.Id,
                    Name = category.Name
                };

                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, createdCategory);
            }
            catch (Exception ex)
            {
                // Log the exception here (ex) using a logging framework
                return StatusCode(500, new { details = ex.Message });
            }
        }


        // PUT: api/category/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            if (updateCategoryDto == null)
            {
                return BadRequest(new { message = "Category data is null." });
            }

            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid data.", errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                var existingCategory = await _categoryRepository.GetCategoryByIdAsync(id);

                if (existingCategory == null)
                {
                    return NotFound(new { message = $"Category with Id {id} not found." });
                }

                existingCategory.Name = updateCategoryDto.Name;
                await _categoryRepository.UpdateCategoryAsync(existingCategory, id);

                return Ok(new { message = $"Category with Id {id} updated to {updateCategoryDto.Name}" });
            }
            catch (Exception ex)
            {
                // Log the exception here (ex) using a logging framework
                return StatusCode(500, new { message = "An error occurred while updating the category.", details = ex.Message });
            }
        }


        // DELETE: api/category/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                var existingCategory = await _categoryRepository.GetCategoryByIdAsync(id);

                if (existingCategory == null)
                {
                    return NotFound(new { message = $"Category with Id {id} not found." });
                }

                await _categoryRepository.DeleteCategoryByIdAsync(id);

                return Ok(new { message = "Category deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception here (ex) using a logging framework
                return StatusCode(500, new {    details = ex.Message });
            }
        }
    }
}
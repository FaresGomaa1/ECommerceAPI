using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ECommerceAPI.Repositories.Interfaces;

namespace ECommerceAPI.ValidationAttributes
{
    public class UniqueCategoryNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var categoryName = value as string;

            // Resolve the repository from the validation context
            var repository = validationContext.GetService(typeof(ICategoryRepository)) as ICategoryRepository;

            if (repository == null)
            {
                throw new InvalidOperationException("ICategoryRepository service is not available.");
            }

            var existingCategories = repository.GetAllCategoriesAsync().Result;
            if (existingCategories.Any(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase)))
            {
                return new ValidationResult("Category name must be unique.");
            }

            return ValidationResult.Success;
        }
    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using ECommerceAPI.Models;

namespace ECommerceAPI.CustomValidation
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Check if value is null
            if (value is null)
            {
                return ValidationResult.Success;
            }

            // Retrieve the UserManager service
            var userManager = (UserManager<User>?)validationContext.GetService(typeof(UserManager<User>));
            if (userManager is null)
            {
                throw new InvalidOperationException("UserManager is not registered in the service container.");
            }

            // Convert value to string and check if it's null or empty
            var email = value as string;
            if (string.IsNullOrEmpty(email))
            {
                return ValidationResult.Success;
            }

            // Check if the email already exists
            var user = userManager.FindByEmailAsync(email).Result;
            if (user != null)
            {
                return new ValidationResult("Email is already in use.");
            }

            return ValidationResult.Success;
        }
    }
}
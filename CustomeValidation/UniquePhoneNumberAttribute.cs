using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using ECommerceAPI.Models;

namespace ECommerceAPI.CustomValidation
{
    public class UniquePhoneNumberAttribute : ValidationAttribute
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
            var phoneNumber = value as string;
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return ValidationResult.Success;
            }

            // Check if the phone number already exists
            var user = userManager.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
            if (user != null)
            {
                return new ValidationResult("Phone number is already in use.");
            }

            return ValidationResult.Success;
        }
    }
}
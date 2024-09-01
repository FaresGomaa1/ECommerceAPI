using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using ECommerceAPI.Models;

public class UniqueUsernameAttribute : ValidationAttribute
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
        var username = value as string;
        if (string.IsNullOrEmpty(username))
        {
            return ValidationResult.Success;
        }

        // Check if the username already exists
        var user = userManager.FindByNameAsync(username).Result;
        if (user != null)
        {
            return new ValidationResult("Username is already taken.");
        }

        return ValidationResult.Success;
    }
}
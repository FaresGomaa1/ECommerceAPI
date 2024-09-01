using ECommerceAPI.CustomValidation;
using System.ComponentModel.DataAnnotations;
namespace ECommerceAPI.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [MinLength(3)]
        [NameValidation]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [NameValidation]
        public string LastName { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and include at least one letter, one number, and one special character.")]
        public string Password { get; set; }

        [Required]
        [MinLength(3)]
        [UniqueUsername]
        public string Username { get; set; }
        [UniquePhoneNumber]
        public string Phone { get; set; }
        [UniqueEmail]
        public string Email { get; set; }
        public AddAddressDTO Address { get; set; }
    }
    public class NameValidationAttribute : RegularExpressionAttribute
    {
        public NameValidationAttribute() : base(@"^[a-zA-Z]{3,}$")
        {
            ErrorMessage = "Name must be at least 3 characters long and contain only letters.";
        }
    }
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

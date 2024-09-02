using System.ComponentModel.DataAnnotations;
using ECommerceAPI.ValidationAttributes;

namespace ECommerceAPI.DTOs
{
    public class GetCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateCategoryDto
    {
        [Required]
        [MinLength(2)]
        [UniqueCategoryName]
        public string Name { get; set; }
    }

    public class UpdateCategoryDto
    {
        [Required]
        [MinLength(2)]
        [UniqueCategoryName]
        public string Name { get; set; }
    }
}

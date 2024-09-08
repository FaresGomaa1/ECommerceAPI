using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{
    public class RateDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Range(0, 5, ErrorMessage = "Rate value must be between 0 and 5.")]
        public int Value { get; set; }
    }

    public class RateResponseDTO
    {
        public int RateId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public int Value { get; set; }
    }
}

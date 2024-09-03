using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{
    public class GetCartDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Color { get; set; }
        public int ColorId { get; set; }
        public string Size { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductId { get; set; }
    }
    public class AddCart
        {
            [Required]
            public int ColorId { get; set; }

            [Required]
            public int SizeId { get; set; }

            [Required]
            [Range(1, int.MaxValue)]
            public int Quantity { get; set; } = 1;

            [Required]
            public int ProductId { get; set; }

            [Required]
            public string UserId { get; set; }
        }

}

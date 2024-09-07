using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{
    public class BaseClass
    {
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ColorName { get; set; }
        [Required]
        public string SizeName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "OrderId must be greater than 0.")]
        public int OrderId { get; set; }
    }
    public class OrderDetailsDTO : BaseClass
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
    public class AddOrderDetailsDTO : BaseClass
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }
    }
}

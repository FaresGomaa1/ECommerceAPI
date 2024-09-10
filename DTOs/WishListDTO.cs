using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{
    public class WishListDTO
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal Rate { get; set; }
    }
    public class AddItemToWishList 
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}

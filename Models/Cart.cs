using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    public class Cart :IdBaseClass
    {
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        public int ProductId {  get; set; }
        public Product Product { get; set; }
    }
}

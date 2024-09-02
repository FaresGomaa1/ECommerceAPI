using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    public class Cart : IdBaseClass
    {
        public int Quantity { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}

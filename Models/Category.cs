namespace ECommerceAPI.Models
{
    public class Category :BaseClass
    {
        public ICollection<Product> Products { get; set; }
    }
}

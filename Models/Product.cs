namespace ECommerceAPI.Models
{
    public class Product : BaseClass
    {
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public ICollection<WishList> wishLists { get; set; }
        public ICollection<Cart> carts { get; set; }
        public ICollection<Rate> rates { get; set; }
        public ICollection<ColorSizeProduct> ColorSizeProducts { get; set; }
        public ICollection<Photo> Photos { get; set; }

    }
}

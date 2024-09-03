namespace ECommerceAPI.Models
{
    public class Color : BaseClass
    {
        public ICollection<ColorSizeProduct> ColorSizeProducts { get; set; }
        public ICollection<Cart> Carts { get; set; }
    }
}

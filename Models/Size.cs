namespace ECommerceAPI.Models
{
    public class Size : BaseClass
    {
        public ICollection<ColorSizeProduct> ColorSizeProducts { get; set; }
        public ICollection<Cart> Carts { get; set; }
    }
}

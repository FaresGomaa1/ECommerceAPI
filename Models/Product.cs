namespace ECommerceAPI.Models
{
    public class Product : BaseClass
    {
        public ICollection<WishList> wishLists { get; set; }
        public ICollection<Cart> carts { get; set; }
        public ICollection<Rate> rates { get; set; }
    }
}

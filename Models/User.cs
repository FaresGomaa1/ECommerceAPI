namespace ECommerceAPI.Models
{
    public class User
    {
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public ICollection<WishList> wishLists { get; set; }
        public ICollection<Cart> carts { get; set; }
        public ICollection<Rate> rates { get; set; }
    }
}

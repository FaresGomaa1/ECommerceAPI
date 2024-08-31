using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI.Models
{
    public class User: IdentityUser
    {
        public ICollection<Address> Addresses { get; set; }
        public ICollection<WishList> wishLists { get; set; }
        public ICollection<Cart> carts { get; set; }
        public ICollection<Rate> rates { get; set; }
        public ICollection<Order> orders { get; set; }  
    }
}

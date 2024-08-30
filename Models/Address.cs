namespace ECommerceAPI.Models
{
    public class Address :IdBaseClass
    {
        public string Country { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public ICollection<User> Users { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{

    public class AddAddressDTO
    {
        [Required]
        public string Country { get; set; }
        public string AddressLine { get; set; }
        [Required]
        public string City { get; set; }
        public string State { get; set; }
    }

    public class GetAddressDTO : AddAddressDTO
    {
        public int Id { get; set; }
    }
}
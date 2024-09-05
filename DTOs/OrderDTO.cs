using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{
    public class OrderBaseDTO
    {
        [Required]
        [UniqueInvoiceNumber]
        public string InvoiceNumber { get; set; }

        [Required]
        public DateTime OpeningDate { get; set; } = DateTime.Now;

        public string Comments { get; set; }

        [Required]
        public int AddressId { get; set; }
    }
    public class GetOrderDTO : OrderBaseDTO
    {
        public int Id { get; set; }
        public DateTime ClosingDate { get; set; }
    }

    public class AddOrderDTO : OrderBaseDTO
    {
        public string userId { get; set; }
    }

    public class UpdateOrderDTO
    {
        public DateTime ClosingDate { get; set; }
        public string Comments { get; set; }
        [Required]
        public int AddressId { get; set; }
    }

}

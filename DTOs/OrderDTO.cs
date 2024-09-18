using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{
    public class OrderBaseDTO
    {
        [Required]
        //[UniqueInvoiceNumber]
        public string InvoiceNumber { get; set; }
        public DateTime OpeningDate { get; set; } = DateTime.Now;
        public string Comments { get; set; } = "some comment";
        [Required]
        public int AddressId { get; set; }
        
    }
    public class GetOrderDTO : OrderBaseDTO
    {
        public int Id { get; set; }
        public DateTime ClosingDate { get; set; }
        public string Status { get; set; }
    }

    public class AddOrderDTO : OrderBaseDTO
    {
        [Required]
        public string userId { get; set; }
        public string Status { get; set; } = "Pending";

    }

    public class UpdateOrderDTO
    {
        public DateTime ClosingDate { get; set; }
        public string Comments { get; set; }
        [RegularExpression(@"^(Pending|Rejected|Approved)$", ErrorMessage = "Status must be either 'Pending', 'Rejected', or 'Approved'.")]
        public string Status { get; set; }
        [Required]
        public int AddressId { get; set; }
    }

}

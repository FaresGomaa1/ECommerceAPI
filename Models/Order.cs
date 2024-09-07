using ECommerceAPI.Models;
using System.ComponentModel.DataAnnotations;

public class Order : IdBaseClass
{
    [Required]
    [StringLength(50, ErrorMessage = "Invoice number cannot exceed 50 characters.")]
    public string InvoiceNumber { get; set; }
    [Required]
    public DateTime OpeningDate { get; set; }
    public DateTime ClosingDate { get; set; }
    [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters.")]
    public string Comments { get; set; }
    [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
    public string Status { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public int AddressId { get; set; }
    public Address Address { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
}

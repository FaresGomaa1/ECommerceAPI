namespace ECommerceAPI.DTOs
{
    public class GetCartDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductId { get; set; }
    }
    public class AddCart
    {
        public int Quantity { get; set; } = 1;
        public int ProductId { get; set; }
        public string UserId { get; set; }
    }

}

namespace ECommerceAPI.Models
{
    public class OrderDetail : IdBaseClass
    {
        public string ProductName {  get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int OrderId {  get; set; }
        public Order Order { get; set; }
    }
}

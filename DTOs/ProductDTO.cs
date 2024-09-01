namespace ECommerceAPI.DTOs
{
    public class GetAllProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public decimal Price { get; set; }
        public decimal AverageRating {get; set; }
    }
}

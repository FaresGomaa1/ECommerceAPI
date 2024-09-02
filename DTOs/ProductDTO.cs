namespace ECommerceAPI.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal AverageRating { get; set; }
    }

    public class GetAllProductDTO : ProductDTO
    {
        public string Category { get; set; }
        public string PhotoUrl { get; set; }
    }

    public class GetProductDTO : ProductDTO
    {
        public string Description { get; set; }
        public List<string> Photos { get; set; }
        public List<ColorSizeDTO> ColorsAndSizesAndQuantity { get; set; }
    }
}
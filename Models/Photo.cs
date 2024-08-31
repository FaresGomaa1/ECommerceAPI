using ECommerceAPI.Models;

public class Photo
{
    public int Id { get; set; }
    public string Url { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}

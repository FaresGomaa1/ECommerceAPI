using ECommerceAPI.DTOs;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;

namespace ECommerceAPI.Services.Classes
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IRateRepository _ratingRepository;

        public ProductService(
                IProductRepository productRepository,
                IPhotoRepository photoRepository,
                IRateRepository ratingRepository)
        {
            _productRepository = productRepository;
            _photoRepository = photoRepository;
            _ratingRepository = ratingRepository;
        }

        public async Task<List<GetAllProductDTO>> GetAllProducts()
        {
            var allProducts = await _productRepository.GetAllProductsAsync();

            var products = await Task.WhenAll(allProducts.Select(async product =>
            {
                var photo = await _photoRepository.GetFirstPhotoByProductIdAsync(product.Id);
                var rates = await _ratingRepository.GetRateByProductIdAsync(product.Id);
                var averageRating = rates.Any() ? rates.Average(r => r.Value) : 0;

                return new GetAllProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    PhotoUrl = photo?.Url,
                    Price = product.Price,
                    AverageRating = (decimal)averageRating
                };
            }));

            return products.ToList();
        }
    }
}
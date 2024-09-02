using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories.Interfaces;
using ECommerceAPI.Services.Interfaces;

namespace ECommerceAPI.Services.Classes
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IRateRepository _ratingRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IColorSizeProductRepository _colorSizeProductRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ISizeRepository _sizeRepository;

        public ProductService(
                IProductRepository productRepository,
                IPhotoRepository photoRepository,
                IRateRepository ratingRepository,
                ICategoryRepository categoryRepository,
                IColorSizeProductRepository colorSizeProductRepository,
                IColorRepository colorRepository,
                ISizeRepository sizeRepository)
        {
            _productRepository = productRepository;
            _photoRepository = photoRepository;
            _ratingRepository = ratingRepository;
            _categoryRepository = categoryRepository;
            _colorSizeProductRepository = colorSizeProductRepository;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
        }

        public async Task<List<GetAllProductDTO>> GetAllProducts()
        {
            var allProducts = await _productRepository.GetAllProductsAsync();

            var products = new List<GetAllProductDTO>();

            foreach (var product in allProducts)
            {
                var photo = await _photoRepository.GetFirstPhotoByProductIdAsync(product.Id);
                var rates = await _ratingRepository.GetRateByProductIdAsync(product.Id);
                var category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);
                var averageRating = rates.Any() ? rates.Average(r => r.Value) : 0;

                products.Add(new GetAllProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = category.Name,
                    PhotoUrl = photo.Url,
                    Price = product.Price,
                    AverageRating = (decimal)averageRating
                });
            }

            return products;
        }

        public async Task<GetProductDTO> GetProductById(int id)
        {
            // Fetch the product
            var product = await _productRepository.GetProductByIdAsync(id);

            // Fetch related data sequentially to avoid DbContext concurrency issues
            var photos = await _photoRepository.GetPhotosByProductIdAsync(id);
            var rates = await _ratingRepository.GetRateByProductIdAsync(id);
            var averageRating = rates.Any() ? rates.Average(r => r.Value) : 0;

            var photoUrls = photos.Select(photo => photo.Url).ToList();

            // Fetch colors and sizes sequentially
            var colorsAndSizes = await _colorSizeProductRepository.GetAllProductSizesAndColorsAsync(id);
            var colorSizeDTOs = new List<ColorSizeDTO>();

            foreach (var colorSize in colorsAndSizes)
            {
                var colorName = await _colorRepository.GetColorByIdAsync(colorSize.ColorId);
                var sizeName = await _sizeRepository.GetSizeByIdAsync(colorSize.SizeId);

                colorSizeDTOs.Add(new ColorSizeDTO
                {
                    ColorName = colorName.Name,
                    SizeName = sizeName.Name,
                    Quantity = colorSize.Quantity
                });
            }

            // Create and return the DTO
            return new GetProductDTO
            {
                Id = id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Photos = photoUrls,
                AverageRating = (decimal)averageRating,
                ColorsAndSizesAndQuantity = colorSizeDTOs
            };
        }
    }
}
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MVC.Services
{

    public class ProductService
    {
        private readonly IProductRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = productRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Get Products List

        public async Task<List<ProductViewModel>> GetProductsAsync(int Page, int ProductsPerPage, CancellationToken cancellationToken)
        {
            var TotalProducts = await GetQuantityTotalProductsAsync(cancellationToken);
            var ProductList = await _repository.GetProductsList(Page, ProductsPerPage, TotalProducts, cancellationToken);

            List<ProductViewModel> result = new List<ProductViewModel>();

            foreach (var element in ProductList)
            {
                ProductViewModel product = new ProductViewModel()
                {
                    Id = element.Id,
                    SKU = element.SKU,
                    Name = element.Name,
                    Price = element.Price ?? 0,
                    Currency = element.Currency,
                    UrlPictures = GetImageUrls(element.Picture)
                };

                result.Add(product);
            }

            return result;
        }
        public async Task<int> GetQuantityTotalProductsAsync(CancellationToken cancellationToken)
        {
            var TotalProducts = await _repository.GetTotalProductsAsync(cancellationToken);
            return TotalProducts;
        }


        public async Task<ProductViewModel> GetProduct(int Id, CancellationToken cancellationToken)
        {
            var Product = await _repository.GetProductAsync(Id, cancellationToken);

                ProductViewModel result = new ProductViewModel()
                {
                    Id = Product.Id,
                    Description = Product.Description ?? "El producto no tiene descripción.",
                    SKU = Product.SKU,
                    Name = Product.Name,
                    Price = Product.Price ?? 0,
                    Currency = Product.Currency,
                    UrlPictures = GetImageUrls(Product.Picture)
                };

            return result;
        }

        private List<string> GetImageUrls(string picturePath)
        {
            List<string> imageUrls = new List<string>();

            string wwwrootPath = _webHostEnvironment.WebRootPath;
            string replaceRelativeSymbol = picturePath.TrimStart('~', '/');
            string fullPath = Path.Combine(wwwrootPath, replaceRelativeSymbol);


            if (Directory.Exists(fullPath))
            {
                var imageFiles = Directory.GetFiles(fullPath, "*.*", SearchOption.TopDirectoryOnly);

                foreach (var file in imageFiles)
                {
                    string fileName = Path.GetFileName(file);
                    imageUrls.Add($"/{replaceRelativeSymbol}/{fileName}".Replace("//", "/"));
                }
            }
            else if (Uri.IsWellFormedUriString(picturePath, UriKind.Absolute) && (picturePath.StartsWith("http://") || picturePath.StartsWith("https://")))
            {
                imageUrls.Add(picturePath);
            }
            return imageUrls;
        }

        #endregion

        #region Add Products

        public async Task AddProductAsync(ProductViewModel product, IFormFileCollection images, CancellationToken cancellationToken)
        {
            if (images != null && images.Count > 0)
            {
                product.PictureList = await TransformPicturesToArrayByte(images, cancellationToken);
            }

            await _repository.SaveProduct(product, cancellationToken);
        }

        private async Task<PictureListDTO> TransformPicturesToArrayByte(IFormFileCollection images, CancellationToken cancellationToken)
        {
            var imageList = new List<PictureDTO>();

            using (var memoryStream = new MemoryStream())
            {
                foreach (var image in images)
                {
                    if (image.Length > 0)
                    {
                        memoryStream.SetLength(0);
                        await image.CopyToAsync(memoryStream, cancellationToken);
                        var imageBytes = memoryStream.ToArray();
                        var imageName = image.FileName;

                        imageList.Add(new PictureDTO(imageBytes, imageName));
                    }
                }
            }
            return new PictureListDTO(imageList);
        }

        #endregion
    }
}

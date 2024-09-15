﻿using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using MVC.Models;

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
            var productList = await GetProductsPageSelected(Page, ProductsPerPage, cancellationToken);
            return productList;
        }

        private async Task<List<ProductViewModel>> GetProductsPageSelected(int page, int ProductsPerPage, CancellationToken cancellationToken)
        {
            var ProductList = await _repository.GetProductsList(page, ProductsPerPage, cancellationToken);
            List<ProductViewModel> result = new List<ProductViewModel>();

            foreach (var element in ProductList)
            {
                ProductViewModel product = new ProductViewModel()
                {
                    Name = element.Name,
                    Description = element.Description,
                    SKU = element.SKU,
                    Code = element.Code,
                    Currency = element.Currency,
                    Price = element.Price,
                    UrlPictures = await GetImageUrls(element.Picture)
                };

                result.Add(product);
            }

            return result;
        }

        private async Task<List<string>> GetImageUrls(string picturePath)
        {
            List<string> imageUrls = new List<string>();

            if (Directory.Exists(picturePath))
            {
                var imageFiles = Directory.GetFiles(picturePath, "*.*", SearchOption.TopDirectoryOnly);

                foreach (var file in imageFiles)
                {
                    string url = ConvertToUrl(file);
                    imageUrls.Add(url);
                }
            }
            return imageUrls;
        }

        private string ConvertToUrl(string filePath)
        {
            string baseUrl = $"{_webHostEnvironment.WebRootPath}/images/products";
            string fileName = Path.GetFileName(filePath);
            return $"{baseUrl}{fileName}";
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

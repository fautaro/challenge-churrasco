using AutoMapper;
using DataAccess.Interfaces;
using DataAccess.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;

namespace MVC.Services
{

    public class ProductService
    {
        private readonly IProductRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;


        public ProductService(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _repository = productRepository;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        #region Get Products List

        public async Task<List<ProductViewModel>> GetProductsAsync(int Page, int ProductsPerPage, CancellationToken cancellationToken)
        {
            var TotalProducts = await GetQuantityTotalProductsAsync(cancellationToken);
            var ProductList = await _repository.GetProductsList(Page, ProductsPerPage, TotalProducts, cancellationToken);

            var result = _mapper.Map<List<ProductViewModel>>(ProductList);

            foreach (var item in result)
            {
                item.UrlPictures = GetImageUrls(item.BaseFolderImages);
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

            var result = _mapper.Map<ProductViewModel>(Product);
            result.UrlPictures = GetImageUrls(result.BaseFolderImages);

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

        public async Task AddProductAsync(ProductViewModel request, IFormFileCollection images, CancellationToken cancellationToken)
        {

            if (images.Count > 0)
                request.BaseFolderImages = await MoveImagesToServer(images, cancellationToken);

            var product = _mapper.Map<Products>(request);

            await _repository.SaveProduct(product, cancellationToken);
        }

        public async Task<string> MoveImagesToServer(IFormFileCollection images, CancellationToken cancellationToken)
        {
            var BaseImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
            var GuidPicturesFolder = Guid.NewGuid().ToString();
            var ImageFolder = Path.Combine(BaseImageFolder, GuidPicturesFolder);
            var relativeUrl = $"~//images//products//{GuidPicturesFolder}";

            if (!Directory.Exists(ImageFolder))
                Directory.CreateDirectory(ImageFolder);

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var filePath = Path.Combine(ImageFolder, $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}");

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream, cancellationToken);
                    }
                }
            }

            return relativeUrl;
        }
        #endregion
    }
}

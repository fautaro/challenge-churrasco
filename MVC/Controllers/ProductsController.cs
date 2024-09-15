using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;

namespace MVC.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _repository;
        private readonly LoginService _loginService;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductsController(ILogger<ProductsController> logger, IProductRepository repository, LoginService loginService, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _repository = repository;
            _loginService = loginService;
            _webHostEnvironment = webHostEnvironment;
        }
        #region Get Products List
        public async Task<IActionResult> Index(int page, CancellationToken cancellationToken)
        {
            var ProductList = await GetPage(page, cancellationToken);

            return View(ProductList);
        }


        private async Task<List<ProductViewModel>> GetPage(int page, CancellationToken cancellationToken)
        {
            var productsPerPage = 5;
            var ProductList = await _repository.GetProductsList(page, productsPerPage, cancellationToken);
            List<ProductViewModel> result = new List<ProductViewModel>();

            foreach (var element in ProductList)
            {
                ProductViewModel product = new ProductViewModel()
                {
                    Id = element.Id,
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

        #endregion

        #region Add Products
        [HttpPost]
        public async Task<Response> AddProduct(ProductViewModel product, IFormFileCollection images, CancellationToken cancellationToken)
        {
            Response result = new Response{ Success = false };

            if (product == null)
                throw new Exception("El producto no puede estar vacío");

            try
            {
                if (images != null && images.Count > 0)
                {
                    product.PictureList = await TransformPicturesToArrayByte(images, cancellationToken);
                }

                await _repository.SaveProduct(product, cancellationToken);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = $"Ocurrió un error al guardar el producto: {ex.Message}";
            }
            return result;
        }


        private async Task<PictureListDTO> TransformPicturesToArrayByte(IFormFileCollection images, CancellationToken cancellationToken)
        {
            var imageList = new List<ImageDataDTO>();

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

                        imageList.Add(new ImageDataDTO(imageBytes, imageName));
                    }
                }
            }
            return new PictureListDTO(imageList);
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
    }

}
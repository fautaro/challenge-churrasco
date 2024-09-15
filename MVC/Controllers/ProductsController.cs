using DataAccess.Interfaces;
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


        public ProductsController(ILogger<ProductsController> logger, IProductRepository repository, LoginService loginService)
        {
            _logger = logger;
            _repository = repository;
            _loginService = loginService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }


        [HttpPost]
        public async Task<Response<bool>> AddProduct(ProductViewModel product, IFormFileCollection images, CancellationToken cancellationToken)
        {
            Response<bool> result = new Response<bool> { Success = false };

            if (product == null)
                throw new Exception("El producto no puede estar vacío");

            try
            {
                if (images != null && images.Count > 0)
                {

                    product.PictureList = await TransformPictures(images, cancellationToken);
                    await _repository.SaveProduct(product, cancellationToken);
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Ocurrió un error al guardar el producto: {ex.Message}";
            }
            return result;
        }


        private async Task<PictureListViewModel> TransformPictures(IFormFileCollection images, CancellationToken cancellationToken)
        {
            var imageList = new List<ImageData>();

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

                        imageList.Add(new ImageData(imageBytes, imageName));
                    }
                }
            }
            return new PictureListViewModel(imageList);
        }
    }

}
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;

namespace MVC.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly ProductService _productService;


        public ProductsController(ILogger<ProductsController> logger, ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }


        #region Get Products List
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken, int Page = 1, int ProductsPerPage = 4)
        {
            var productList = await _productService.GetProductsAsync(Page, ProductsPerPage, cancellationToken);
            var totalProducts = await _productService.GetQuantityTotalProductsAsync(cancellationToken);

            ViewBag.CurrentPage = Page;
            ViewBag.ProductsPerPage = ProductsPerPage;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / ProductsPerPage);

            return View(productList);
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct(int Id, CancellationToken cancellationToken)
        {
            var Product = await _productService.GetProduct(Id, cancellationToken);

            return PartialView("_ModalProductPartial", Product);
        }

        #endregion

        #region Add Products

        [HttpGet]
        public IActionResult AddProduct(CancellationToken cancellationToken)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel product, IFormFileCollection images, CancellationToken cancellationToken)
        {
            if (product == null)
                throw new Exception("El producto no puede estar vacío.");
            try
            {
                await _productService.AddProductAsync(product, images, cancellationToken);
                TempData["SuccessMessage"] = "Producto guardado correctamente";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error al guardar el producto: {ex.Message}";
            }
            return RedirectToAction("Index");

        }

        #endregion
    }

}
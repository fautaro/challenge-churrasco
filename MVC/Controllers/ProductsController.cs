﻿using DataAccess.Interfaces;
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
        private readonly ProductService _productService;


        public ProductsController(ILogger<ProductsController> logger, ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }


        #region Get Products List
        [HttpGet]
        public async Task<IActionResult> Index(int Page, int ProductsPerPage, CancellationToken cancellationToken)
        {
            var ProductList = await _productService.GetProductsAsync(Page, ProductsPerPage, cancellationToken);

            return View(ProductList);
        }

        #endregion

        #region Add Products
        [HttpPost]
        public async Task<Response> AddProduct(ProductViewModel product, IFormFileCollection images, CancellationToken cancellationToken)
        {
            Response result = new Response{ Success = false };

            if (product == null)
                throw new Exception("El producto no puede estar vacío.");
            try
            {
                await _productService.AddProductAsync(product, images, cancellationToken);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = $"Ocurrió un error al guardar el producto: {ex.Message}";
            }
            return result;
        }

        #endregion
    }

}
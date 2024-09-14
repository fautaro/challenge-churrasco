using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;

namespace MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IUserRepository _repository;
        private readonly LoginService _loginService;


        public ProductsController(ILogger<ProductsController> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }
    }

}
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;

namespace MVC.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IUserRepository _repository;
        private readonly LoginService _loginService;


        public ProductsController(ILogger<ProductsController> logger, IUserRepository repository, LoginService loginService)
        {
            _logger = logger;
            _repository = repository;
            _loginService = loginService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }
    }

}
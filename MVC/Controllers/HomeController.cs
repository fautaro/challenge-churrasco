using DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using System.Diagnostics;
using Churrasco.Models;

namespace Churrasco.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _repository;
        private readonly LoginService _loginService;


        public HomeController(ILogger<HomeController> logger, IUserRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }


        [HttpPost]
        public async Task<LoginViewModel> Login(string username, string password, CancellationToken cancellationToken)
        {

            var loginResult = await _loginService.AuthenticateUser(username, password, cancellationToken);

            if (loginResult.Success)
                RedirectToAction("Products", "Index");

            return loginResult;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using Churrasco.Models;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using System.Diagnostics;

namespace Churrasco.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _repository;
        private readonly LoginService _loginService;


        public HomeController(ILogger<HomeController> logger, IUserRepository repository, LoginService loginService)
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
        public async Task<LoginViewModel> Login(string username, string password, CancellationToken cancellationToken)
        {
            LoginViewModel LoginResult = new LoginViewModel();

            LoginResult = await _loginService.AuthenticateUser(username, password, cancellationToken);

            if (LoginResult.Success)
                RedirectToAction("Products", "Index");

            return LoginResult;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

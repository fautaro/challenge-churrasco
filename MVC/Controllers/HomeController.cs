using Churrasco.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using System.Diagnostics;

namespace Churrasco.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LoginService _loginService;


        public HomeController(ILogger<HomeController> logger, LoginService loginService)
        {
            _logger = logger;
            _loginService = loginService;
        }

        public IActionResult Index()
        {
            //Todo: Corregir para que sea general
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Products");

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TempData["ErrorMessage"] = "Todos los campos son requeridos.";
                return RedirectToAction("Index", "Home");
            }

            var response = await _loginService.AuthenticateUser(username, password, cancellationToken);

            if (response.Success)
                return RedirectToAction("Index", "Products");


            TempData["ErrorMessage"] = response.Message;
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            await _loginService.SignOutAsync(cancellationToken);
            return RedirectToAction("Index", "Home");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

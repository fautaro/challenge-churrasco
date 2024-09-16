using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MVC.Models;
using System.Security.Claims;

namespace MVC.Services
{
    public class LoginService
    {
        private readonly IUserRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CryptoService _cryptoService;


        public LoginService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, CryptoService cryptoService)
        {
            _repository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _cryptoService = cryptoService;
        }

        public async Task<Response> AuthenticateUser(string username, string password, CancellationToken cancellationToken)
        {
            var response = new Response() { Success = false };
            try
            {
                var encriptedPassword = _cryptoService.Encrypt(password);
                var user = await _repository.GetUser(username, encriptedPassword, cancellationToken);

                if (user == null)
                {
                    response.Message = "Los datos ingresados no son correctos.";
                    return response;
                }

                await SignInAsync(user.Username, user.Role.ToLower());
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Ocurrió un error al iniciar sesión.";
            }
            return response;
        }

        private async Task SignInAsync(string Username, string Role)
        {
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, Username),
            new Claim(ClaimTypes.Role, Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }


        public async Task SignOutAsync(CancellationToken cancellationToken)
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}

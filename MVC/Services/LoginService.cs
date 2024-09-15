using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authentication;
using MVC.Models;
using System.Security.Claims;

namespace MVC.Services
{
    public class LoginService
    {
        private readonly IUserRepository _repository;
        private readonly HttpContext _httpContextAccessor;
        private readonly CryptoService _cryptoService;


        public LoginService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, CryptoService cryptoService)
        {
            _repository = userRepository;
            _httpContextAccessor = httpContextAccessor.HttpContext;
            _cryptoService = cryptoService;
        }

        public async Task<Response> AuthenticateUser(string username, string password, CancellationToken cancellationToken)
        {
            var response = new Response() {Success = false };
            try
            {
                var encriptedPassword = await _cryptoService.Encrypt(password);
                var user = await _repository.GetUser(username, encriptedPassword, cancellationToken);

                if (user == null)
                {
                    response.Message = "Los datos ingresados no son correctos.";
                    return response;
                }

                await SignInAsync(user.Username, user.Role);
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

            var identity = new ClaimsIdentity(claims, "CookieAuthentication");
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.SignInAsync("CookieAuthentication", principal);
        }


        public async Task SignOutAsync()
        {
            await _httpContextAccessor.SignOutAsync("CookieAuthentication");
        }
    }
}

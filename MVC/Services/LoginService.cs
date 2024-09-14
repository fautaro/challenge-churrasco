using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using MVC.Models;
using System.Security.Claims;

namespace MVC.Services
{
    public class LoginService
    {
        private readonly IUserRepository _repository;
        private readonly HttpContext _httpContextAccessor;


        public LoginService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = userRepository;
            _httpContextAccessor = httpContextAccessor.HttpContext;
        }

        public async Task<LoginViewModel> AuthenticateUser(string username, string password, CancellationToken cancellationToken)
        {
            LoginViewModel response = new LoginViewModel();
            var user = await _repository.GetUser(username, password, cancellationToken);

            if (user == null)
            {
                response.Message = "Los datos ingresados no son correctos.";
                return response;
            }

            await SignInAsync(user.Username, user.Role);
            response.Success = true;
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

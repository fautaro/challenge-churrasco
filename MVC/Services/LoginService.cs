using DataAccess.Interfaces;
using Domain.Entities;
using MVC.Models;

namespace MVC.Services
{
    public class LoginService
    {
        private readonly IUserRepository _repository;

        public LoginService(IUserRepository userRepository)
        {
            _repository = userRepository;
        }

        public async Task<LoginViewModel> AuthenticateUser(string username, string password, CancellationToken cancellationToken)
        {
            LoginViewModel response = new LoginViewModel();
            var user = _repository.GetUser(username, password, cancellationToken);

            if (user == null)
            {
                response.Message = "Los datos ingresados no son correctos.";
                return response;
            }

            response.Success = true;
            return response;
        }
    }
}

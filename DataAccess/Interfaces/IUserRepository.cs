using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUser(string usernameOrEmail, string password, CancellationToken cancellationToken);
    }
}

using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly IApplicationDbContext _context;

        public UserRepository(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<User> GetUser(string usernameOrEmail, string password, CancellationToken cancellationToken)
        {

            var user = await _context.Users.
                AsNoTracking().
                Where(e => (e.Username.Equals(usernameOrEmail) || e.Email.Equals(usernameOrEmail)) && e.Password.Equals(password) && e.Active && e.Role.Equals("admin"))
               .FirstOrDefaultAsync(cancellationToken);
            return user;
        }
    }
}

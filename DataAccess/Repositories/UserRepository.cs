using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly IApplicationDbContext _context;

        public UserRepository(IApplicationDbContext context)
        {
            _context = context;
        }
    }
}

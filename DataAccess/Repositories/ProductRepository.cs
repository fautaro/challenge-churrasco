using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        readonly IApplicationDbContext _context;

        public ProductRepository(IApplicationDbContext context)
        {
            _context = context;
        }

    }
}

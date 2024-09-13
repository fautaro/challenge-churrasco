using DataAccess.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        readonly IApplicationDbContext _context;

        public ProductRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Products>> GetProductsList(int ProductsPerPage, int? pageSelected, CancellationToken cancellationToken)
        {
            var ProductsCount = await _context.Products.AsNoTracking().CountAsync(cancellationToken);
            var PagesCount = (int)Math.Ceiling((double)ProductsCount / ProductsPerPage);

            if (ProductsCount > 0)
            {
                var page = pageSelected ?? 1;
                var skip = (page - 1) * ProductsPerPage;

                return await _context.Products
                    .AsNoTracking()
                    .Skip(skip)
                    .Take(ProductsPerPage)
                    .ToListAsync(cancellationToken);
            }

            return new List<Products>();
        }


        public async Task SaveProduct(Products product, CancellationToken cancellationToken)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}

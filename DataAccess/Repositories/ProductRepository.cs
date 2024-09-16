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
        #region Get Products 
        public async Task<Products> GetProductAsync(int Id, CancellationToken cancellationToken)
        {
            var Product = await _context.Products
                .Where(e => e.Id.Equals(Id))
                .FirstOrDefaultAsync(cancellationToken);

            return Product;
        }
        public async Task<List<Products>> GetProductsList(int Page, int ProductsPerPage, int ProductsCount, CancellationToken cancellationToken)
        {

            if (ProductsCount > 0)
            {
                var skip = (Page - 1) * ProductsPerPage;

                return await _context.Products
                    .AsNoTracking()
                    .Skip(skip)
                    .Take(ProductsPerPage)
                    .ToListAsync(cancellationToken);
            }

            return new List<Products>();
        }

        public async Task<int> GetTotalProductsAsync(CancellationToken cancellationToken)
        {
            var ProductsCount = await _context.Products.AsNoTracking().CountAsync(cancellationToken);
            return ProductsCount;
        }

        #endregion

        #region Save Products 

        public async Task SaveProduct(Products product, CancellationToken cancellationToken)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
        }

        #endregion

    }
}

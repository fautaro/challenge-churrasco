using DataAccess.Models;
using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Products>> GetProductsList(int Page, int ProductsPerPage, int ProductCounts, CancellationToken cancellationToken);
        Task<int> GetTotalProductsAsync(CancellationToken cancellationToken);
        Task SaveProduct(ProductViewModel request, CancellationToken cancellationToken);
    }
}

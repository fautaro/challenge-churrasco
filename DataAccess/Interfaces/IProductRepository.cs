using DataAccess.Models.ViewModels;
using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Products>> GetProductsList(int ProductsPerPage, int? pageSelected, CancellationToken cancellationToken);
        Task SaveProduct(ProductViewModel request, CancellationToken cancellationToken);
    }
}

using Domain.Entities;

namespace DataAccess.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Products>> GetProductsList(int ProductsPerPage, int? pageSelected, CancellationToken cancellationToken);
    }
}

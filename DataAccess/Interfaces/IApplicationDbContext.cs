using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Products> Products { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}

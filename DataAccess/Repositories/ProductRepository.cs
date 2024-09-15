using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Models.ViewModels;
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

        public async Task<List<Products>> GetProductsList(int Page, int ProductsPerPage, CancellationToken cancellationToken)
        {
            var ProductsCount = await _context.Products.AsNoTracking().CountAsync(cancellationToken);
            var PagesCount = (int)Math.Ceiling((double)ProductsCount / ProductsPerPage);

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


        public async Task SaveProduct(ProductViewModel request, CancellationToken cancellationToken)
        {
            if (request.PictureList is not null)
                await SaveImages(request.PictureList, cancellationToken);

            //Todo: Migrar a automapper - o clase especial para mapeo manual
            var product = new Products()
            {
                SKU = request.SKU,
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Picture = request.PictureList != null ? request.PictureList.ImageFolder : string.Empty,
                Currency = request.Currency

            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task SaveImages(PictureListDTO pictureList, CancellationToken cancellationToken)
        {
            foreach (var image in pictureList.Images)
            {
                if (image.ImageBytes.Length > 0)
                {
                    if (!Directory.Exists(pictureList.ImageFolder))
                        Directory.CreateDirectory(pictureList.ImageFolder);

                    var filePath = Path.Combine(pictureList.ImageFolder, $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}");

                    await File.WriteAllBytesAsync(filePath, image.ImageBytes, cancellationToken);
                }
            }
        }
    }
}

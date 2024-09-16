
namespace DataAccess.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public long SKU { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }

        public List<string>? UrlPictures { get; set; }
        public string? BaseFolderImages { get; set; }

    }
}

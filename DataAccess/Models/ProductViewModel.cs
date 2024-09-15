using DataAccess.Models.ViewModels;

namespace DataAccess.Models
{
    public class ProductViewModel
    {
        public long SKU { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }

        public List<string>? UrlPictures { get; set; }

        public PictureListDTO? PictureList { get; set; }
    }
}

namespace DataAccess.Models.ViewModels
{
    public class PictureListDTO
    {
        public string BaseImageFolder { get; set; }
        public string ImageGuid { get; set; }
        public string ImageFolder { get; set; }
        public List<ImageDataDTO> Images { get; set; }
        public PictureListDTO(List<ImageDataDTO> images)
        {
            BaseImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            ImageGuid = Guid.NewGuid().ToString();
            ImageFolder = Path.Combine(BaseImageFolder, ImageGuid);
            Images = images;
        }
    }
}

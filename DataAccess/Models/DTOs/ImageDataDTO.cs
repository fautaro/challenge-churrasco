namespace DataAccess.Models.ViewModels
{
    public class ImageDataDTO
    {
        public byte[] ImageBytes { get; set; }
        public string FileName { get; set; }

        public ImageDataDTO(byte[] imageBytes, string fileName)
        {
            ImageBytes = imageBytes;
            FileName = fileName;
        }
    }

}

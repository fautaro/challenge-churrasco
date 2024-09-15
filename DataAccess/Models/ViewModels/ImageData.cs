namespace DataAccess.Models.ViewModels
{
    public class ImageData
    {
        public byte[] ImageBytes { get; set; }
        public string FileName { get; set; }

        public ImageData(byte[] imageBytes, string fileName)
        {
            ImageBytes = imageBytes;
            FileName = fileName;
        }
    }

}

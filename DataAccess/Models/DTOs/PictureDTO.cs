namespace DataAccess.Models.ViewModels
{
    public class PictureDTO
    {
        public byte[] ImageBytes { get; set; }
        public string FileName { get; set; }

        public PictureDTO(byte[] imageBytes, string fileName)
        {
            ImageBytes = imageBytes;
            FileName = fileName;
        }
    }

}

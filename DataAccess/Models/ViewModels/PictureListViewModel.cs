﻿namespace DataAccess.Models.ViewModels
{
    public class PictureListViewModel
    {
        public string BaseImageFolder { get; set; }
        public string ImageGuid { get; set; }
        public string ImageFolder { get; set; }
        public List<ImageData> Images { get; set; }
        public PictureListViewModel(List<ImageData> images)
        {
            BaseImageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            ImageGuid = Guid.NewGuid().ToString();
            ImageFolder = Path.Combine(BaseImageFolder, ImageGuid);
            Images = images;
        }
    }
}

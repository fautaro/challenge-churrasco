using AutoMapper;
using DataAccess.Models;
using Domain.Entities;

namespace DataAccess.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductViewModel, Products>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src => src.BaseFolderImages != null ? src.BaseFolderImages : string.Empty));

            CreateMap<Products, ProductViewModel>()
                .ForMember(dest => dest.BaseFolderImages, opt => opt.MapFrom(src => src.Picture))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price ?? 0))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? "El producto no tiene descripción."));

        }
    }

}

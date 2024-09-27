using AutoMapper;
using WebAPI.Dtos;
using WebAPI.Models;

namespace WebAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<City, CityUpdateDto>().ReverseMap();
            CreateMap<Property, PropertyDto>().ReverseMap();
            CreateMap<Photo, PhotoDto>().ReverseMap();
            CreateMap<Property, PropertyListDto>()
                .ForMember(d => d.City, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(d => d.Country, opt => opt.MapFrom(src => src.City.Country))
                .ForMember(d => d.PropertyType, opt => opt.MapFrom(src => src.PropertyType.Name))
                .ForMember(d => d.FurnishingType, opt => opt.MapFrom(src => src.FurnishingType.Name))
                .ForMember(d => d.Photo, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsPrimary).ImageUrl));
            CreateMap<Property, PropertyDetailDto>()
               .ForMember(d => d.City, opt => opt.MapFrom(src => src.City.Name))
               .ForMember(d => d.Country, opt => opt.MapFrom(src => src.City.Country))
               .ForMember(d => d.PropertyType, opt => opt.MapFrom(src => src.PropertyType.Name))
               .ForMember(d => d.FurnishingType, opt => opt.MapFrom(src => src.FurnishingType.Name));
            CreateMap<PropertyType, KeyValuePairDto>().ReverseMap();
            CreateMap<FurnishingType, KeyValuePairDto>().ReverseMap();
            CreateMap<Like, LikeDto>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
               .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(src => src.Property.Name));
        }
    }
}

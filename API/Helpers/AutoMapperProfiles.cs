using System.Linq;
using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(
                src => src.Photos.FirstOrDefault(x => x.IsMain).Url)).ReverseMap();
            CreateMap<Order, OrderDto>();
            CreateMap<Order, CustomerOrderDto>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<OrderedProducts, OrderedProductsDto>();
            CreateMap<QuantityDto, OrderedProducts>();
            CreateMap<ProductPhoto, ProductPhotoDto>();
            CreateMap<Order, AdminOrderDto>();
            CreateMap<MemberDto, AppUser>().ReverseMap();
        }
    }
}
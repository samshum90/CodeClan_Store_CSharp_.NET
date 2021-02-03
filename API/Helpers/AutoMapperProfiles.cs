using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<Order, OrderDto>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<OrderedProducts, OrderedProductsDto>();
            CreateMap<QuantityDto, OrderedProducts>();
            CreateMap<ProductPhoto, ProductPhotoDto>();
            CreateMap<Order, AdminOrderDto>();
            CreateMap<MemberDto, AppUser>().ReverseMap();
        }
    }
}
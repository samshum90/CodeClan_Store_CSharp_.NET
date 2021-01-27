using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<EditOrderDto, Order>();
            CreateMap<OrderedProducts, OrderedProductsDto>();
            CreateMap<QuantityDto, OrderedProducts>();
            CreateMap<ProductPhoto, ProductPhotoDto>();
        }
    }
}
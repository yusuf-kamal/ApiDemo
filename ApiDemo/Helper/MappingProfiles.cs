using ApiDemo.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAgregate;

namespace ApiDemo.Helper
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d=>d.ProductBrand,option=>option.MapFrom(src=>src.ProductBrand.Name))
                .ForMember(d=>d.ProductType,option=>option.MapFrom(src=>src.ProductType.Name))
                .ForMember(d=>d.PictureUrl,option=>option.MapFrom<ProductUrlResolver>());
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem,BasketItemDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<ShippingAddress, ShippingAddressDto>().ReverseMap();
            CreateMap<Order, OrderDelatilsDto>()
                .ForMember(dest => dest.DeliveryMethod, options => options.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShippingPrice, options => options.MapFrom(src => src.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.itemOrdered.ProductItemId))
                .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.itemOrdered.ProductName))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom(src => src.itemOrdered.PictureUrl))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom<OrderItemUrlResolver>());

        }
    }
}

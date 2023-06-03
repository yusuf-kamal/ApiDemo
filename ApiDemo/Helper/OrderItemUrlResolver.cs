using ApiDemo.Dtos;
using AutoMapper;
using Core.Entities.OrderAgregate;

namespace ApiDemo.Helper
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.itemOrdered.PictureUrl))
                return _configuration["ApiUrl"] + source.itemOrdered.PictureUrl;
            return null;
        }
    }
}

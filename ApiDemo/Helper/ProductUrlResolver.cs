using ApiDemo.Dtos;
using AutoMapper;
using AutoMapper.Execution;
using Core.Entities;

namespace ApiDemo.Helper
{
    public class ProductUrlResolver : IValueResolver<Product, ProductDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl)) 
                return _configuration["ApiUrl"]+source.PictureUrl;
            return null;
        }
    }
}

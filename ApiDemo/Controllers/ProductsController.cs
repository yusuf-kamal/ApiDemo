using ApiDemo.Dtos;
using ApiDemo.Helper;
using ApiDemo.Response_Module;
using AutoMapper;
using Core.Entities;
using Core.Entities.Interface;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiDemo.Controllers
{
   
    public class ProductsController : BaseController
    {
        private readonly IGenericRepo<Product> _productRepo;
        private readonly IGenericRepo<ProductBrand> _productBrandRepo;
        private readonly IGenericRepo<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        //private readonly IProductRepo _productRepo;

        public ProductsController(/*IProductRepo productRepo*/
            IGenericRepo<Product> productRepo,
            IGenericRepo<ProductBrand> ProductBrandRepo,
            IGenericRepo<ProductType> ProductTypeRepo,
            IMapper mapper)
        {
            _productRepo = productRepo;
            _productBrandRepo = ProductBrandRepo;
            _productTypeRepo = ProductTypeRepo;
            _mapper = mapper;
            //_productRepo = productRepo;
        }

        [Cached(10)]
        [HttpGet("GetProducts")]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery]ProductSpecParams productSpecParams)
        {
            var specs = new ProductsWithTypeAndBrandSpecification(productSpecParams);
            var countspec = new ProductWithFilterForCountSpecification(productSpecParams);
            var totalItems = await _productRepo.CountAsync(countspec);
            var products= await _productRepo.ListAsync(specs);
            var mapproduct = _mapper.Map<IReadOnlyList<ProductDto>>(products);

            var paginateDate = new Pagination<ProductDto>(productSpecParams.PageIndex, productSpecParams.pageSize, totalItems, mapproduct);
            return Ok(paginateDate);
        }

        [HttpGet("GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var specs = new ProductsWithTypeAndBrandSpecification(id);

            var product =await _productRepo.GetEntityWithSpecification(specs);


             if(product == null)
            return NotFound(new ApiResponse(404));

            var mapproduct = _mapper.Map<ProductDto>(product);
             return Ok(mapproduct);

        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductsBrands()
        {
            var ProductBrands=await _productBrandRepo.ListAllAsync();
            if(ProductBrands==null)
                return NotFound();
            return Ok(ProductBrands);
        }
        [HttpGet("Type")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductType()
        {
            var ProductType=await _productTypeRepo.ListAllAsync();
            if (ProductType == null)
                return NotFound();
            return Ok(ProductType);
        }
    }
}

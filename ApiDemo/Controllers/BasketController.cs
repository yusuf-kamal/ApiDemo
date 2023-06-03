using ApiDemo.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : BaseController
    {
        private readonly IBasketRepo _basketRepo;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepo basketRepo ,IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketId( string id)
        {
            var basket=await _basketRepo.GetBasketAsync(id);
            return Ok(basket?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>>UpdateBasket(CustomerBasketDto customerBasketDto)
        {
            var basket = _mapper.Map<CustomerBasket>(customerBasketDto);
            var updatBasket=await _basketRepo.UpdateBasketAsync(basket);
            return Ok(updatBasket);
        }
        [HttpDelete]
        public async Task DeleteBasketById(string id)
            =>await _basketRepo.DeleteBasketAsync(id);
    }
}

using ApiDemo.Dtos;
using ApiDemo.Extentions;
using ApiDemo.Response_Module;
using AutoMapper;
using Core.Entities.Interface;
using Core.Entities.OrderAgregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDemo.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User.RetriveEmailPrincipale();
            var address = _mapper.Map<ShippingAddress>(orderDto.Address);
            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);
            if (order is null)
                return BadRequest(new ApiResponse(400, "Problem with create order"));
            return Ok(order);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDelatilsDto>> GetOrderByIdForUser(int id)
        {
            var email = HttpContext.User.RetriveEmailPrincipale();
            var order=await _orderService.GetOrderById(id,email);
            if (order is null)
                return NotFound(new ApiResponse(404, "Order is not exist"));
            return Ok(_mapper.Map<OrderDelatilsDto>(order));
        }
        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<IReadOnlyList<OrderDelatilsDto>>> GetOrdersForUsers()
        {
            var email = HttpContext.User.RetriveEmailPrincipale();
            var orders = await _orderService.GetOrdersForUserAsyncByEmail(email);
             return Ok(_mapper.Map<IReadOnlyList<OrderDelatilsDto>>(orders));
        }
        [HttpGet("DeliveryMethod")]

        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }
    }
}

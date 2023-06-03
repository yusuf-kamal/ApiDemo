using Core.Entities;
using Core.Entities.Interface;
using Core.Entities.OrderAgregate;
using Core.Specifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Core.Entities.Product;

namespace Infrastructure.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IUnitOFWork _unitOFWork;
        private readonly IBasketRepo _basketRepo;
        private readonly IConfiguration _configuration;

        public PaymentServices( IUnitOFWork unitOFWork,IBasketRepo basketRepo, IConfiguration configuration)
        {
            _unitOFWork = unitOFWork;
            _basketRepo = basketRepo;
            _configuration = configuration;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];
            var basket=await _basketRepo.GetBasketAsync(basketId);
            if (basket is null)
                return null;
            var shippingPrice = 0m;
            if (basket.DleiveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOFWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DleiveryMethodId.Value);
                shippingPrice=deliveryMethod.Price;

            }
            foreach (var item in basket.BasketItems)
            {
                var productItem = await _unitOFWork.Repository<Product>().GetByIdAsync(item.Id);
                if(item.Price!=productItem.price)
                    item.Price= productItem.price;

            }
            var service = new PaymentIntentService();
            PaymentIntent intent;
            if(string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quntity * (item.Price * 100)) + (long)(shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
              intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quntity * (item.Price * 100)) + (long)(shippingPrice * 100),

                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }
            await _basketRepo.UpdateBasketAsync(basket);
            return basket;
                 
        }

        public async Task<Order> UpdateOrderPaymentFailed(string PaymentIntentId)
        {
            var spec = new OrderWithPaymentSpecification(PaymentIntentId);
            var order = await _unitOFWork.Repository<Order>().GetEntityWithSpecification(spec);
            if (order is null)
                return null;
            order.OrderStatus=OrderStatus.PaymentFailed;
            _unitOFWork.Repository<Order>().Update(order);
            await _unitOFWork.Complete();
            return order;
        }

        public async Task<Order> UpdateOrderPaymentSucceeded(string PaymentIntentId)
        {

            var spec = new OrderWithPaymentSpecification(PaymentIntentId);
            var order = await _unitOFWork.Repository<Order>().GetEntityWithSpecification(spec);
            if (order is null)
                return null;
            order.OrderStatus = OrderStatus.PaymentReceived;
            _unitOFWork.Repository<Order>().Update(order);
            await _unitOFWork.Complete();
            return order;
        }
    }
}

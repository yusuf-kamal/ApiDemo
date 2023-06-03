using Core.Entities;
using Core.Entities.Interface;
using Core.Entities.OrderAgregate;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepo _basketRepo;
        private readonly IUnitOFWork _unitOFWork;
        private readonly IPaymentServices _paymentServices;

        public OrderService( IBasketRepo basketRepo, IUnitOFWork unitOFWork,IPaymentServices paymentServices)
        {
           _basketRepo = basketRepo;
           _unitOFWork = unitOFWork;
           _paymentServices = paymentServices;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, ShippingAddress address)
        {
            var basket = await _basketRepo.GetBasketAsync(basketId);
            var items=new List<OrderItem>();
            foreach (var item in basket.BasketItems)
            {
                var productItem = await _unitOFWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered=new ProductItemOrdered(productItem.Id,productItem.Name,productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered,productItem.price,item.Quntity);
                items.Add(orderItem);
            } 
            var deliverMethod=await _unitOFWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            var subtotal = items.Sum(item => item.Price * item.Quantity);
            var spec = new OrderWithPaymentSpecification(basket.PaymentIntentId);
            var existingOrder = await _unitOFWork.Repository<Order>().GetEntityWithSpecification( spec);
            if(existingOrder!= null)
            {
                _unitOFWork.Repository<Order>().Delete(existingOrder);
                await _paymentServices.CreateOrUpdatePaymentIntent(basketId);
            }

            var order = new Order(buyerEmail,address, deliverMethod,items,subtotal,basket.PaymentIntentId);
            _unitOFWork.Repository<Order>().Add(order);
            var result = await _unitOFWork.Complete();

            if (result <= 0)
                return null;
            //await _basketRepo.DeleteBasketAsync(basketId);
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
       => await _unitOFWork.Repository<DeliveryMethod>().ListAllAsync();

        public async Task<Order> GetOrderById(int id, string buyerEmail)
        {
            var orderSpec = new OrderWithItemSpecification(id, buyerEmail);
            return await _unitOFWork.Repository<Order>().GetEntityWithSpecification(orderSpec);  
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsyncByEmail(string buyerEmail)
        {
            var orderSpec = new OrderWithItemSpecification( buyerEmail);
            return await _unitOFWork.Repository<Order>().ListAsync(orderSpec);
        }
    }
}

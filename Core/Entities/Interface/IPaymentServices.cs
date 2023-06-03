using Core.Entities.OrderAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Interface
{
    public interface IPaymentServices
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId); 
        Task<Order>UpdateOrderPaymentSucceeded(string PaymentIntentId);
        Task<Order> UpdateOrderPaymentFailed(string PaymentIntentId);
    }
}

using Core.Entities.OrderAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class OrderWithPaymentSpecification : BaseSpecification<Order>
    {
        public OrderWithPaymentSpecification(string PaymentIntentId) : base(order=>order.PaymentIntentId==PaymentIntentId)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderAgregate
{
    public class Order:BaseEntity
    {
        public Order()
        {

        }
        public Order(string buyerEmail, ShippingAddress shippedAddress,
            DeliveryMethod deliveryMethod, IReadOnlyList<OrderItem> orderItems,
            decimal subTotal,string? paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippedAddress = shippedAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public int Id { get; set; }
        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; }=DateTimeOffset.Now;
        public ShippingAddress ShippedAddress { get; set; }
        public DeliveryMethod  DeliveryMethod { get; set;}
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal SubTotal { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public string? PaymentIntentId { get; set; }
        public decimal GetTotal()
            => SubTotal + DeliveryMethod.Price;
    }
}

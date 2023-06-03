using Core.Entities;

namespace ApiDemo.Dtos
{
    public class CustomerBasketDto
    {
        public string Id { get; set; }
        public int? DleiveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
        public List<BasketItemDto>BasketItems{ get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
    }
}

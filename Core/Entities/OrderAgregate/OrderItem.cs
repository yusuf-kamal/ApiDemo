namespace Core.Entities.OrderAgregate
{
    public class OrderItem:BaseEntity
    {
        public OrderItem()
        {

        }
        public OrderItem( ProductItemOrdered itemOrdered, decimal price, int quantity)
        {
           
            this.itemOrdered = itemOrdered;
            Price = price;
            Quantity = quantity;
        }

        public int Id { get; set; }
        public ProductItemOrdered  itemOrdered { get; set; }
        public decimal Price { get; set; }
        public int Quantity  { get; set; }
    }
}
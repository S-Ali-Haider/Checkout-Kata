namespace CheckoutKata.Core.Domain
{
    public class Offer
    {
        public int Quantity { get; }
        public int Price { get; }

        public Offer(int quantity, int price)
        {
            if (quantity <= 0) 
            {
                throw new ArgumentException("Quantity must be > 0", nameof(quantity));
            }

            if (price <= 0) 
            {
                throw new ArgumentException("Price must be > 0", nameof(price));
            }

            Quantity = quantity;
            Price = price;
        }
    }
}

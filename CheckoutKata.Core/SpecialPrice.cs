namespace CheckoutKata.Core
{
    public class SpecialPrice
    {
        public int DiscountQuantity { get; }
        public int DiscountedPrice { get; }

        public SpecialPrice(int discountQuantity, int discountedPrice)
        {
            DiscountQuantity = discountQuantity;
            DiscountedPrice = discountedPrice;
        }
    }
}

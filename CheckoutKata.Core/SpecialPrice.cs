namespace CheckoutKata.Core
{
    public class SpecialPrice
    {
        public int DiscountQuantity;
        public int DiscountedPrice;

        public SpecialPrice(int discountQuantity, int discountedPrice)
        {
            DiscountQuantity = discountQuantity;
            DiscountedPrice = discountedPrice;
        }
    }
}

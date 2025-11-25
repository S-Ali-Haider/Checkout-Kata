namespace CheckoutKata.Core
{
    public interface IPriceCalculator
    {
        int Calculate(PricingRule rule, int qty);
    }
}

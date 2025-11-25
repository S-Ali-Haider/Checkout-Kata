namespace CheckoutKata.Core
{
    public interface IPricingCalculatorResolver
    {
        IPriceCalculator Resolve(PricingRule rule);
    }
}

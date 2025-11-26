using CheckoutKata.Core.Domain;
using CheckoutKata.Core.Interfaces;

namespace CheckoutKata.Core.Logic.Resolvers
{
    public interface IPricingCalculatorResolver
    {
        IPriceCalculator Resolve(PricingRule rule);
    }
}

using CheckoutKata.Core.Domain;
using CheckoutKata.Core.Interfaces;
using CheckoutKata.Core.Logic.Calculators;

namespace CheckoutKata.Core.Logic.Resolvers
{
    public class DefaultPricingCalculatorResolver : IPricingCalculatorResolver
    {
        private readonly IPriceCalculator _unitCalculator = new NoOfferPriceCalculator();
        private readonly IPriceCalculator _singleCalculator = new SingleOfferPriceCalculator();
        private readonly IPriceCalculator _multiCalculator = new MultiOfferPriceCalculator();

        public IPriceCalculator Resolve(PricingRule rule)
        {
            if (rule == null) 
            { 
                throw new ArgumentNullException(nameof(rule)); 
            }

            if (rule.Offers == null || rule.Offers.Count == 0)
            {
                return _unitCalculator;
            }

            if (rule.Offers.Count == 1)
            {
                return _singleCalculator;
            }

            return _multiCalculator;
        }
    }
}

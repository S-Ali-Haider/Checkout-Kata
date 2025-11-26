using CheckoutKata.Core.Domain;
using CheckoutKata.Core.Interfaces;

namespace CheckoutKata.Core.Logic.Calculators
{
    internal class NoOfferPriceCalculator : IPriceCalculator
    {
        public int Calculate(PricingRule rule, int qty)
        {
            if (rule == null) 
            { 
                throw new ArgumentNullException(nameof(rule)); 
            }

            return rule.UnitPrice * qty;
        }
    }
}

namespace CheckoutKata.Core
{
    internal class SingleOfferPriceCalculator : IPriceCalculator
    {
        public int Calculate(PricingRule rule, int qty)
        {
            if (rule == null) 
            { 
                throw new ArgumentNullException(nameof(rule)); 
            }

            if (rule.Offers == null || rule.Offers.Count == 0)
            { 
                throw new ArgumentException("No offers for bundle calculator", nameof(rule)); 
            }

            var offer = rule.Offers[0];
            int bundles = qty / offer.Quantity;
            int remainder = qty % offer.Quantity;
            return bundles * offer.Price + remainder * rule.UnitPrice;
        }
    }
}

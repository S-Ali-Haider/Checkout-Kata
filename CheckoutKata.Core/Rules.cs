namespace CheckoutKata.Core
{
    public class Rules : IPricingRules
    {
        private readonly Dictionary<string, PricingRule> _pricingRules;

        public Rules()
        {
            _pricingRules = new();
        }

        /// <summary>
        /// Adds a new Rule to existing rules
        /// </summary>
        /// <param name="item"></param>
        /// <param name="unitPrice"></param>
        /// <param name="specialPrices"></param>
        public void AddRule(string item, int unitPrice, SpecialPrice[]? specialPrices)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                throw new ArgumentException($"Item is null or Empty");
            }

            if (specialPrices != null)
            {
                foreach(SpecialPrice sp in specialPrices)
                {
                    if (sp.DiscountedPrice <= 0)
                        throw new ArgumentException($"There must be an offer and discounted price must be > 0");

                    if (sp.DiscountQuantity <= 0)
                        throw new ArgumentException($"There must be an offer and discounted quantity must be > 0");
                }
            }

            if (unitPrice <= 0)
            {
                throw new ArgumentException($"UnitPrice must be > 0");
            }

            item = item.ToUpperInvariant();

            _pricingRules[item] = new PricingRule(item, unitPrice, specialPrices);
        }

        /// <summary>
        /// Returns the price of a single sku 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Price in int if it exists, otherwise 0</returns>
        public int GetPrice(string item)
        {
            if (string.IsNullOrWhiteSpace(item)) 
            {
                throw new ArgumentException($"Item is not given.");
            }

            if (!_pricingRules.TryGetValue(item.ToUpperInvariant(), out var rule))
            {
                throw new ArgumentException($"There is no pricing rule for this item: {item}");
            }

            return rule.UnitPrice;
        }

        /// <summary>
        /// Returns offers associated with this item
        /// </summary>
        /// <param name="item"></param>
        /// <returns>SpecialPrices each containing discountedQuantity and discountedPrice if any offer exists, otherwise null.</returns>
        public SpecialPrice[]? GetOffers(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                throw new ArgumentException($"Item is not given.");
            }

            if (!_pricingRules.TryGetValue(item.ToUpperInvariant(), out var rule))
            {
                throw new ArgumentException($"There is no offer for this item: {item}");
            }

            return rule.SpecialPrice;
        }


        /// <summary>
        /// Checks if an item is valid based on whether it was added with some price or not.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>True if item exists otherwise false.</returns>
        public bool IsValidItem(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
            { 
                return false; 
            }

            return _pricingRules.ContainsKey(item.ToUpperInvariant());
        }
    }
}

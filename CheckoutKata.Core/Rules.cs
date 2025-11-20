namespace CheckoutKata.Core
{
    public class Rules : IPricingRules
    {
        private PricingRule[] _pricingRules;

        public Rules()
        {
            _pricingRules = Array.Empty<PricingRule>();
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

            PricingRule? existingRule = _pricingRules.FirstOrDefault(x => x.Item == item);

            if (existingRule != null)
            {
                existingRule.UnitPrice = unitPrice; 
                existingRule.SpecialPrice = specialPrices;

                return;
            }

            PricingRule rule = new PricingRule(item, unitPrice, specialPrices);
            _pricingRules = _pricingRules.Append(rule).ToArray();
        }

        /// <summary>
        /// Returns the price of a single sku 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Price in int if it exists, otherwise 0</returns>
        public int GetPrice(string item)
        {
            PricingRule? rule = _pricingRules.FirstOrDefault(x => x.Item == item);
            if (rule == null)
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
            PricingRule? rule = _pricingRules.FirstOrDefault(x => x.Item == item);
            if (rule == null)
            {
                Console.WriteLine($"There is no pricing rule for this item: {item}");
                return null;
            }

            if (rule.SpecialPrice == null)
            {
                Console.WriteLine($"There is no offer for this item: {item}");
                return null;
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
            if (_pricingRules.FirstOrDefault(x => x.Item == item) == null)
            {
                return false;
            }

            return true;
        }
    }
}

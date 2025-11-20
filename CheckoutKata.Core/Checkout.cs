using Checkout_Kata;

namespace CheckoutKata.Core
{
    public class Checkout : ICheckout
    {
        private readonly IPricingRules _pricingRules;
        private readonly Dictionary<string, int> _scannedItems = new Dictionary<string, int>();

        public Checkout(IPricingRules pricingRules)
        {
            _pricingRules = pricingRules;
        }

        /// <summary>
        /// Adds an item to _scannedItems dictionary
        /// </summary>
        /// <param name="item"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Scan(string item)
        {
            if (!_pricingRules.IsValidItem(item))
                throw new ArgumentException($"Invalid item: {item}");

            item = item.ToUpperInvariant();

            if (_scannedItems.ContainsKey(item))
                _scannedItems[item] += 1;
            else
                _scannedItems[item] = 1;
        }

        /// <summary>
        /// Returns total price of all scanned items till now.
        /// </summary>
        /// <returns>Total price in int</returns>
        public int GetTotalPrice()
        {
            int total = 0;

            foreach (KeyValuePair<string, int> kv in _scannedItems)
            {
                string sku = kv.Key;
                int qty = kv.Value;

                int unitPrice = _pricingRules.GetPrice(sku);
                SpecialPrice[] offers = _pricingRules.GetOffers(sku) ?? Array.Empty<SpecialPrice>();

                var dp = new int[qty + 1];
                dp[0] = 0;

                for (int i = 1; i <= qty; i++)
                {
                    dp[i] = dp[i - 1] + unitPrice;

                    if (offers != null)
                    {
                        foreach (var sp in offers)
                        {
                            if (sp == null) 
                            { 
                                continue; 
                            }

                            if (i >= sp.DiscountQuantity)
                            {
                                dp[i] = Math.Min(dp[i], dp[i - sp.DiscountQuantity] + sp.DiscountedPrice);
                            }
                        }
                    }
                }

                total += dp[qty];
            }

            return total;
        }
    }
}

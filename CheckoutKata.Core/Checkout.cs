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

        public void Scan(string item)
        {
            if (!_pricingRules.IsValidItem(item))
                throw new ArgumentException($"Invalid item: {item}");

            if (_scannedItems.ContainsKey(item))
                _scannedItems[item] += 1;
            else
                _scannedItems[item] = 1;
        }

        public int GetTotalPrice()
        {
            int total = 0;

            foreach (KeyValuePair<string, int> kv in _scannedItems)
            {
                total += kv.Value * _pricingRules.GetPrice(kv.Key);
            }

            return total;
        }
    }
}

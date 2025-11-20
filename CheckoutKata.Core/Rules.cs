namespace CheckoutKata.Core
{
    public class Rules:IPricingRules
    {
        private Dictionary<string, int> _prices = new Dictionary<string, int>();

        public Rules()
        {
            _prices["A"] = 50;
            _prices["B"] = 30;
            _prices["C"] = 20;
            _prices["D"] = 15;
        }

        public int GetPrice(string item)
        {
            return _prices[item];
        }

        public bool IsValidItem(string item)
        {
            return _prices.ContainsKey(item);
        }
    }
}

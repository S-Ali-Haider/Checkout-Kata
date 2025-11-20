namespace CheckoutKata.Core
{
    public class Rules : IPricingRules
    {
        private PricingRule[] _pricingRules;

        public Rules()
        {
            _pricingRules = new PricingRule[0];

            AddRule("A", 50, new SpecialPrice(3, 130));
            AddRule("B", 30, new SpecialPrice(2, 45));
            AddRule("C", 20, null);
            AddRule("D", 15, null);
        }

        public void AddRule(string item, int quantity, SpecialPrice? specialPrice)
        {
            PricingRule rule = new PricingRule(item, quantity, specialPrice);
            _pricingRules = _pricingRules.Append(rule).ToArray();
        }

        public int GetPrice(string item)
        {
            PricingRule? rule = _pricingRules.FirstOrDefault(x => x.Item == item);
            if (rule == null)
            {
                Console.WriteLine($"There is no pricing rule for this item: {item}");
                return 0;
            }

            return rule.UnitPrice;
        }

        public SpecialPrice? GetOffer(string item)
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

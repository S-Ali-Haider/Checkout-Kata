using CheckoutKata.Core.Interfaces;
using CheckoutKata.Core.Logic.Resolvers;

namespace CheckoutKata.Core.Services
{
    public class PricingService : IPricingService
    {
        private readonly IPricingRepository _repo;
        private readonly IPricingCalculatorResolver _resolver;

        public PricingService(IPricingRepository repo, IPricingCalculatorResolver resolver)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public bool IsValidSku(string sku) => _repo.IsValidSku(sku);

        public int CalculateTotal(IReadOnlyDictionary<string, int> basket)
        {
            if (basket == null) 
            { 
                throw new ArgumentNullException(nameof(basket)); 
            }

            int total = 0;

            foreach (var kv in basket)
            {
                var sku = kv.Key;
                var qty = kv.Value;

                var rule = _repo.GetRule(sku);
                var calculator = _resolver.Resolve(rule);
                total += calculator.Calculate(rule, qty);
            }

            return total;
        }
    }
}

using CheckoutKata.Core.Domain;
using CheckoutKata.Core.Interfaces;
using System.Collections.Concurrent;

namespace CheckoutKata.Core.Data
{
    public class InMemoryPricingRepository : IPricingRepository
    {
        // Case insensitive concurrent dictionary for concurrent reads and writes
        private readonly ConcurrentDictionary<string, PricingRule> _rules = 
            new ConcurrentDictionary<string, PricingRule>(StringComparer.OrdinalIgnoreCase);

        public void AddRule(PricingRule rule)
        {
            if (rule == null) 
            { 
                throw new ArgumentNullException(nameof(rule)); 
            }

            _rules[rule.Sku] = rule;
        }


        public bool IsValidSku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku)) 
            { 
                return false; 
            }

            return _rules.ContainsKey(sku);
        }


        public PricingRule GetRule(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku)) 
            { 
                throw new ArgumentException("sku required", nameof(sku)); 
            }

            if (!_rules.TryGetValue(sku, out var rule)) 
            { 
                throw new ArgumentException($"No pricing rule for sku '{sku}'"); 
            }

            return rule;
        }
    }
}

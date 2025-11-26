using CheckoutKata.Core.Interfaces;

namespace CheckoutKata.Core.Services
{
    public class Checkout : ICheckout
    {
        private readonly IPricingService _pricingService;
        private readonly Dictionary<string, int> _scanned = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        public Checkout(IPricingService pricingService)
        {
            _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
        }

        public void Scan(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
            { 
                throw new ArgumentException("item required", nameof(item)); 
            }

            var sku = item.Trim();

            if (!_pricingService.IsValidSku(sku)) 
            { 
                throw new ArgumentException($"Invalid SKU: {item}"); 
            }

            if (_scanned.ContainsKey(sku)) 
            { 
                _scanned[sku]++; 
            }
            else 
            { 
                _scanned[sku] = 1; 
            }
        }

        public int GetTotalPrice()
        {
            // Passing a copy to preserve encapsulation
            return _pricingService.CalculateTotal(new Dictionary<string, int>(_scanned, StringComparer.OrdinalIgnoreCase));
        }
    }
}

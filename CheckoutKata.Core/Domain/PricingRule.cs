namespace CheckoutKata.Core.Domain
{
    public class PricingRule
    {
        public string Sku { get; }
        public int UnitPrice { get; }
        public IReadOnlyList<Offer> Offers { get; }

        public PricingRule(string sku, int unitPrice, IEnumerable<Offer>? offers = null)
        {
            if (string.IsNullOrWhiteSpace(sku))
            { 
                throw new ArgumentException("sku required", nameof(sku)); 
            }

            if (unitPrice <= 0) 
            { 
                throw new ArgumentException("unitPrice must be > 0", nameof(unitPrice)); 
            }

            // We dont need to normalize casing here. The repository handles that with case insensitive comparer
            Sku = sku.Trim();
            UnitPrice = unitPrice;
            Offers = (offers ?? Enumerable.Empty<Offer>()).ToArray();
        }
    }
}

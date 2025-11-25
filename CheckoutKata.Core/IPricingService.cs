namespace CheckoutKata.Core
{
    public interface IPricingService
    {
        int CalculateTotal(IReadOnlyDictionary<string, int> basket);

        bool IsValidSku(string sku);
    }
}

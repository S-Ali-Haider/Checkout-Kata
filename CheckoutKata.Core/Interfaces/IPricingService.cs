namespace CheckoutKata.Core.Interfaces
{
    public interface IPricingService
    {
        int CalculateTotal(IReadOnlyDictionary<string, int> basket);

        bool IsValidSku(string sku);
    }
}

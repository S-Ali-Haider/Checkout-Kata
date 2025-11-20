namespace CheckoutKata.Core
{
    public interface IPricingRules
    {
        int GetPrice(string item);
        bool IsValidItem(string item);
    }
}

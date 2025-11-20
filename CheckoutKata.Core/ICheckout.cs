namespace Checkout_Kata
{
    public interface ICheckout
    {
        void Scan(string item);
        int GetTotalPrice();
    }
}

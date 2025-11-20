using Checkout_Kata;

namespace CheckoutKata.Core
{
    public class Checkout : ICheckout
    {
        private readonly Rules _rules;

        public Checkout(Rules rules)
        {
            _rules = rules;
        }

        public void Scan(string item)
        {

        }

        public int GetTotalPrice()
        {
            return -1;
        }
    }
}

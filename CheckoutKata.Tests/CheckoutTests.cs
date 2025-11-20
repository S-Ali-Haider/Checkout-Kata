using CheckoutKata.Core;

namespace CheckoutKata.Tests
{
    public class CheckoutTests
    {
        [Fact]
        public void EmptyBasket_TotalIsZero()
        {
            var rules = new Rules();
            var checkout = new Checkout(rules);

            Assert.Equal(0, checkout.GetTotalPrice());
        }
    }
}

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

        [Theory]
        [InlineData("A", 50)]
        [InlineData("AB", 80)]
        [InlineData("CDBA", 115)]
        public void BasicTotals_ReturnExpected(string items, int expected)
        {
            var rules = new Rules();
            var checkout = new Checkout(rules);

            foreach (var c in items) 
            {
                checkout.Scan(c.ToString());
            }

            Assert.Equal(expected, checkout.GetTotalPrice());
        }
    }
}

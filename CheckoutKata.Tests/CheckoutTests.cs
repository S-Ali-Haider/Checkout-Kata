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

        [Fact]
        public void ThreeA_For130()
        {
            var rules = new Rules();
            var checkout = new Checkout(rules); 

            checkout.Scan("A"); 
            checkout.Scan("A"); 
            checkout.Scan("A");

            Assert.Equal(130, checkout.GetTotalPrice());
        }

        [Fact]
        public void AAABBB_Mixed()
        {
            var rules = new Rules();
            var checkout = new Checkout(rules);

            foreach (var c in "AAABBB")
            { 
                checkout.Scan(c.ToString()); 
            }

            Assert.Equal(205, checkout.GetTotalPrice());
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("A", 50)]
        [InlineData("AB", 80)]
        [InlineData("CDBA", 115)]
        [InlineData("AA", 100)]
        [InlineData("AAA", 130)]
        [InlineData("AAAA", 180)]
        [InlineData("AAAAA", 230)]
        [InlineData("AAAAAA", 260)]
        [InlineData("AAAB", 160)]
        [InlineData("AAABB", 175)]
        [InlineData("AAABBD", 190)]
        [InlineData("DABABA", 190)]
        public void OfferRules(string items, int expected)
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

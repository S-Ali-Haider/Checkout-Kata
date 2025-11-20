using CheckoutKata.Core;

namespace CheckoutKata.Tests
{
    public class CheckoutTests
    {
        private Rules _rules;

        public CheckoutTests()
        {
            _rules = new Rules();
            _rules.AddRule("A", 50, new SpecialPrice[] { new SpecialPrice(3, 130) });
            _rules.AddRule("B", 30, new SpecialPrice[] { new SpecialPrice(2, 45) });
            _rules.AddRule("C", 20, null);
            _rules.AddRule("D", 15, null);
        }

        // helper method for scanning items given as a string
        private static void ScanString(Checkout checkout, string items)
        {
            if (items == null) 
                return;

            foreach (var ch in items)
                checkout.Scan(ch.ToString());
        }

        [Fact]
        public void EmptyBasket_TotalIsZero()
        {
            var checkout = new Checkout(_rules);

            Assert.Equal(0, checkout.GetTotalPrice());
        }

        [Theory]
        [InlineData("A", 50)]
        [InlineData("AB", 80)]
        [InlineData("CDBA", 115)]
        public void BasicTotals_ReturnExpected(string items, int expected)
        {
            var checkout = new Checkout(_rules);

            ScanString(checkout, items);

            Assert.Equal(expected, checkout.GetTotalPrice());
        }

        [Fact]
        public void ThreeA_For130()
        {
            var checkout = new Checkout(_rules); 

            checkout.Scan("A"); 
            checkout.Scan("A"); 
            checkout.Scan("A");

            Assert.Equal(130, checkout.GetTotalPrice());
        }

        [Fact]
        public void AAABBB_Mixed()
        {
            var checkout = new Checkout(_rules);

            ScanString(checkout, "AAABBB");

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
            var checkout = new Checkout(_rules);

            ScanString(checkout, items);

            Assert.Equal(expected, checkout.GetTotalPrice());
        }


        [Fact]
        public void Scan_EmptyString()
        {
            var checkout = new Checkout(_rules);

            Assert.Throws<ArgumentException>(() => checkout.Scan(string.Empty));
        }

        [Fact]
        public void Scan_MultiCharacterSku()
        {
            var checkout = new Checkout(_rules);

            // Can only scan one at a time so this should be invalid.
            Assert.Throws<ArgumentException>(() => checkout.Scan("AB"));
        }

        [Fact]
        public void AddRule_Empty()
        {
            Assert.Throws<ArgumentException>(() => _rules.AddRule("", 10, null));
            Assert.Throws<ArgumentException>(() => _rules.AddRule("   ", 10, null));
        }

        [Fact]
        public void AddRule_NegativePrice()
        {
            Assert.Throws<ArgumentException>(() => _rules.AddRule("X", -5, null));
        }

        [Fact]
        public void AddRule_ZeroOrNegativeDiscountQuantity()
        {
            // Zero discount quantity should be invalid
            Assert.Throws<ArgumentException>(() => _rules.AddRule("Y", 10, new SpecialPrice[] { new SpecialPrice(0, 20) }));
            Assert.Throws<ArgumentException>(() => _rules.AddRule("Y", 10, new SpecialPrice[] { new SpecialPrice(-3, 20) }));
        }

        [Fact]
        public void AddRule_NegativeDiscountedPrice()
        {
            Assert.Throws<ArgumentException>(() => _rules.AddRule("Z", 10, new SpecialPrice[] { new SpecialPrice(3, -100) }));
        }

        [Fact]
        public void GetPrice_UnknownSku()
        {
            Assert.Throws<ArgumentException>(() => _rules.GetPrice("UNKNOWN_SKU"));
        }

        [Fact]
        public void GetOffer_UnknownSku()
        {
            Assert.Throws<ArgumentException>(() => _rules.GetOffers("UNKNOWN_SKU"));
        }

        [Fact]
        public void GetOffer_NoOffer()
        {
            var offer = _rules.GetOffers("C");
            Assert.Null(offer);
        }

        [Fact]
        public void GetTotalPrice_IsIdempotent()
        {
            var checkout = new Checkout(_rules);
            ScanString(checkout, "AAB");

            var first = checkout.GetTotalPrice();
            var second = checkout.GetTotalPrice();
            Assert.Equal(first, second);

            checkout.Scan("A");

            var expectedCheckout = new Checkout(_rules);

            ScanString(expectedCheckout, "AAAB"); 
            Assert.Equal(expectedCheckout.GetTotalPrice(), checkout.GetTotalPrice());
        }

        [Fact]
        public void LargeQuantity_OverflowTest()
        {
            var checkout = new Checkout(_rules);

            for (int i = 0; i < 1000; i++) 
            { 
                checkout.Scan("A"); 
            }
     
            var total = checkout.GetTotalPrice();
            Assert.True(total >= 0);
        }

        [Fact]
        public void AddRule_ReplacesExistingRule()
        {
            // Add rule for X as 10
            _rules.AddRule("X", 10, null);
            Assert.Equal(10, _rules.GetPrice("X"));

            // Add again with different price, should update 
            _rules.AddRule("X", 20, null);
            Assert.Equal(20, _rules.GetPrice("X"));
        }

        [Fact]
        public void MultipleOffers_DP()
        {
            var rules = new Rules();

            rules.AddRule("A", 50, new SpecialPrice[] {
                new SpecialPrice(3, 130),
                new SpecialPrice(5, 200)
            });

            rules.AddRule("B", 30, new SpecialPrice[] {
                new SpecialPrice(2, 45),
                new SpecialPrice(4, 80)
            });

            rules.AddRule("C", 20, null);

            rules.AddRule("D", 15, null);

            {
                var checkout = new Checkout(rules);
                for (int i = 0; i < 5; i++) checkout.Scan("A");
                Assert.Equal(200, checkout.GetTotalPrice());
            }

            {
                var checkout = new Checkout(rules);
                for (int i = 0; i < 8; i++) checkout.Scan("A");
                Assert.Equal(330, checkout.GetTotalPrice());
            }

            {
                var checkout = new Checkout(rules);
                for (int i = 0; i < 9; i++) checkout.Scan("A");
                Assert.Equal(380, checkout.GetTotalPrice());
            }

            {
                var checkout = new Checkout(rules);
                for (int i = 0; i < 4; i++) checkout.Scan("B");
                Assert.Equal(80, checkout.GetTotalPrice());
            }

            {
                var checkout = new Checkout(rules);
                for (int i = 0; i < 6; i++) checkout.Scan("B");
                Assert.Equal(125, checkout.GetTotalPrice());
            }

            {
                var checkout = new Checkout(rules);
                for (int i = 0; i < 5; i++) checkout.Scan("A");
                for (int i = 0; i < 4; i++) checkout.Scan("B");
                Assert.Equal(280, checkout.GetTotalPrice());
            }
        }
    }
}

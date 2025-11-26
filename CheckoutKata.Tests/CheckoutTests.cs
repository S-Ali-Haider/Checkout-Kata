using CheckoutKata.Core.Data;
using CheckoutKata.Core.Domain;
using CheckoutKata.Core.Interfaces;
using CheckoutKata.Core.Logic.Resolvers;
using CheckoutKata.Core.Services;

namespace CheckoutKata.Tests
{
    public class CheckoutTests
    {
        private readonly IPricingRepository _repo;
        private readonly IPricingService _service;

        public CheckoutTests()
        {
            _repo = new InMemoryPricingRepository();

            _repo.AddRule(new PricingRule("A", 50, new[] { new Offer(3, 130) }));
            _repo.AddRule(new PricingRule("B", 30, new[] { new Offer(2, 45) }));
            _repo.AddRule(new PricingRule("C", 20));
            _repo.AddRule(new PricingRule("D", 15));

            var resolver = new DefaultPricingCalculatorResolver();
            _service = new PricingService(_repo, resolver);
        }

        // Helper method for scanning items given as a string
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
            var checkout = new Checkout(_service);

            Assert.Equal(0, checkout.GetTotalPrice());
        }

        [Theory]
        [InlineData("A", 50)]
        [InlineData("AB", 80)]
        [InlineData("CDBA", 115)]
        public void BasicTotals_ReturnExpected(string items, int expected)
        {
            var checkout = new Checkout(_service);

            ScanString(checkout, items);

            Assert.Equal(expected, checkout.GetTotalPrice());
        }

        [Fact]
        public void ThreeA_For130()
        {
            var checkout = new Checkout(_service); 

            checkout.Scan("A"); 
            checkout.Scan("A"); 
            checkout.Scan("A");

            Assert.Equal(130, checkout.GetTotalPrice());
        }

        [Fact]
        public void AAABBB_Mixed()
        {
            var checkout = new Checkout(_service);

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
            var checkout = new Checkout(_service);

            ScanString(checkout, items);

            Assert.Equal(expected, checkout.GetTotalPrice());
        }


        [Fact]
        public void Scan_EmptyString()
        {
            var checkout = new Checkout(_service);

            Assert.Throws<ArgumentException>(() => checkout.Scan(string.Empty));
        }

        [Fact]
        public void Scan_MultiCharacterSku()
        {
            var checkout = new Checkout(_service);

            // Can only scan one at a time so this should be invalid
            Assert.Throws<ArgumentException>(() => checkout.Scan("AB"));
        }

        [Fact]
        public void AddRule_Empty()
        {
            Assert.Throws<ArgumentException>(() => _repo.AddRule(new PricingRule("", 10, null)));
            Assert.Throws<ArgumentException>(() => _repo.AddRule(new PricingRule("   ", 10, null)));
        }

        [Fact]
        public void AddRule_NegativePrice()
        {
            Assert.Throws<ArgumentException>(() => _repo.AddRule(new PricingRule("X", -5, null)));
        }

        [Fact]
        public void AddRule_ZeroOrNegativeDiscountQuantity()
        {
            // Zero discount quantity should be invalid
            Assert.Throws<ArgumentException>(() => _repo.AddRule(new PricingRule("Y", 10, new Offer[] { new Offer(0, 20) })));
            Assert.Throws<ArgumentException>(() => _repo.AddRule(new PricingRule("Y", 10, new Offer[] { new Offer(-3, 20) })));
        }

        [Fact]
        public void AddRule_NegativeDiscountedPrice()
        {
            Assert.Throws<ArgumentException>(() => _repo.AddRule(new PricingRule("Z", 10, new Offer[] { new Offer(3, -100) })));
        }

        [Fact]
        public void GetPrice_UnknownSku()
        {
            Assert.Throws<ArgumentException>(() => _repo.GetRule("UNKNOWN_SKU").UnitPrice);
        }

        [Fact]
        public void GetOffer_UnknownSku()
        {
            Assert.Throws<ArgumentException>(() => _repo.GetRule("UNKNOWN_SKU").Offers);
        }

        [Fact]
        public void GetOffer_NoOffer()
        {
            var offer = _repo.GetRule("C").Offers;
            Assert.Equal(offer, Enumerable.Empty<Offer>().ToArray());
        }

        [Fact]
        public void GetTotalPrice_IsIdempotent()
        {
            var checkout = new Checkout(_service);
            ScanString(checkout, "AAB");

            var first = checkout.GetTotalPrice();
            var second = checkout.GetTotalPrice();
            Assert.Equal(first, second);

            checkout.Scan("A");

            var expectedCheckout = new Checkout(_service);

            ScanString(expectedCheckout, "AAAB"); 
            Assert.Equal(expectedCheckout.GetTotalPrice(), checkout.GetTotalPrice());
        }

        [Fact]
        public void LargeQuantity_OverflowTest()
        {
            var checkout = new Checkout(_service);

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
            _repo.AddRule(new PricingRule("X", 10, null));
            Assert.Equal(10, _repo.GetRule("X").UnitPrice);

            // Add again with different price, should update 
            _repo.AddRule(new PricingRule("X", 20, null));
            Assert.Equal(20, _repo.GetRule("X").UnitPrice);
        }

        [Fact]
        public void MultipleOffers_DP()
        {
            InMemoryPricingRepository _newRepo;
            _newRepo = new InMemoryPricingRepository();

            var resolver = new DefaultPricingCalculatorResolver();

            PricingService _newService;
            _newService = new PricingService(_newRepo, resolver);

            _newRepo.AddRule(new PricingRule("A", 50, new Offer[] {
                new Offer(3, 130),
                new Offer(5, 200)
            }));

            _newRepo.AddRule(new PricingRule("B", 30, new Offer[] {
                new Offer(2, 45),
                new Offer(4, 80)
            }));

            _newRepo.AddRule(new PricingRule("C", 20, null));

            _newRepo.AddRule(new PricingRule("D", 15, null));

            {
                var checkout = new Checkout(_newService);
                for (int i = 0; i < 5; i++) checkout.Scan("A");
                Assert.Equal(200, checkout.GetTotalPrice());
            }

            {
                var checkout = new Checkout(_newService);
                for (int i = 0; i < 8; i++) checkout.Scan("A");
                Assert.Equal(330, checkout.GetTotalPrice());
            }

            {
                var checkout = new Checkout(_newService);
                for (int i = 0; i < 9; i++) checkout.Scan("A");
                Assert.Equal(380, checkout.GetTotalPrice());
            }

            {
                var checkout = new Checkout(_newService);
                for (int i = 0; i < 4; i++) checkout.Scan("B");
                Assert.Equal(80, checkout.GetTotalPrice());
            }

            {
                var checkout = new Checkout(_newService);
                for (int i = 0; i < 6; i++) checkout.Scan("B");
                Assert.Equal(125, checkout.GetTotalPrice());
            }

            {
                var checkout = new Checkout(_newService);
                for (int i = 0; i < 5; i++) checkout.Scan("A");
                for (int i = 0; i < 4; i++) checkout.Scan("B");
                Assert.Equal(280, checkout.GetTotalPrice());
            }
        }
    }
}

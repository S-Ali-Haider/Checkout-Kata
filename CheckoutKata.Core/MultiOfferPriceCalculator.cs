namespace CheckoutKata.Core
{
    internal class MultiOfferPriceCalculator : IPriceCalculator
    {
        public int Calculate(PricingRule rule, int quantity)
        {
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            var availableOffers = rule.Offers ?? Array.Empty<Offer>();

            if (quantity == 0)
            {
                return 0;
            }

            // DP array: minimumPrice[q] stores the minimum cost to purchase 'q' items
            var minimumPrice = new int[quantity + 1];
            minimumPrice[0] = 0; // Cost to buy 0 items is 0

            // Iterate through every quantity (q) from 1 up to the total quantity needed
            for (int q = 1; q <= quantity; q++)
            {
                // Start by assuming the cheapest way to get 'q' items is:
                // (Cheapest way to get q-1 items) + (Unit Price of the last item)
                minimumPrice[q] = minimumPrice[q - 1] + rule.UnitPrice;

                // Apply offers
                foreach (var offer in availableOffers)
                {
                    int offerSize = offer.Quantity;
                    int offerCost = offer.Price;

                    // Check if the current offer can be applied to reach quantity 'q'
                    if (offerSize <= q)
                    {
                        // Calculate the cost using this offer:
                        // (Cheapest way to get q - offerSize items) + (Cost of the current offer)
                        int costIfUsingOffer = minimumPrice[q - offerSize] + offerCost;

                        // Update the minimum price for quantity 'q'
                        if (costIfUsingOffer < minimumPrice[q])
                        {
                            minimumPrice[q] = costIfUsingOffer;
                        }
                    }
                }
            }

            // The final answer is the minimum cost stored at the required quantity index
            return minimumPrice[quantity];
        }
    }
}

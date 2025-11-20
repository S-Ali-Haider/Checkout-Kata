# Checkout-Kata

This is my implementation of the Checkout Kata problem, built using C#, .NET, and TDD from the start.
The goal of the kata is to scan items (SKUs), apply pricing rules, and calculate the total price which may include special offers.

I started with a simple greedy approach, then refactored the pricing logic to a dynamic programming solution so it supports multiple offers per SKU.

# Features

- Scan items one by one using Scan(string item)
- Get the total price at any time with GetTotalPrice()
- Supports:
    - unit prices
    - multiple discount offers per SKU
- Strong input validation (invalid SKUs, empty input, negative prices, etc.)
- Fully TDD-driven with multiple tests
- SOLID-aware design

# Tests

- All tests run through GitHub Actions (.github/workflows/dotnet.yml).
- Tests includes:
    - Basic totals
    - Offer rules
    - Idempotency
    - Input validation
    - Multiple offers per SKU (DP algorithm)
    - Overflow tests
    - Rule replacement tests
 
# Notes

- The dynamic programming approach was chosen so the checkout can always find the best price even if multiple discount rules overlap.
- Future improvements could include writing the IPricingEngine interface for even cleaner SOLID layering.

# Checkout-Kata

This is my implementation of the Checkout Kata problem, built using C#, .NET, and TDD from the start.
The goal of the kata is to scan items (SKUs), apply pricing rules, and calculate the total price which may include special offers.

The pricing logic has been refactored to follow clean separation of concerns.  
`Checkout` now only tracks scanned items while a dedicated pricing service and calculator classes compute totals.

# Features

- Scan items using `Scan(string sku)`
- Get running totals with `GetTotalPrice()`
- Pricing rules support:
  - unit prices
  - single-offer products
  - multiple-offer products (dynamic programming)
- Clear separation between:
  - Checkout
  - PricingService
  - Pricing calculators (strategy pattern)
- Input validation for invalid SKUs, empty values, and malformed rules
- Fully covered with tests

# Architecture

- **IPriceCalculator**: interface for all pricing strategies  
- **NoOffer / SingleOffer / MultiOffer calculators**: each handles its own pricing logic  
- **DefaultPricingCalculatorResolver**: selects the right calculator for each rule  
- **PricingService**: resolves the calculator and returns the final price  
- **Checkout**: only collects quantities and asks PricingService for totals

This keeps the code understandable, testable and easy to extend if new pricing types are added later.

# Tests

Covers:
- Basic pricing
- Single and multiple offer rules
- Edge cases and invalid data
- DP behavior for multi-offer pricing
- End to end Checkout scenarios

# Notes

The dynamic programming calculator ensures the best price when several offers overlap.  
The resolver and calculator classes keep the design clean without unnecessary cognitive complexity.

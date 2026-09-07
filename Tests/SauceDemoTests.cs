using System.Collections.Generic;
using NUnit.Framework;
using SeleniumTests.Pages;

namespace SeleniumTests.Tests
{
    [TestFixture]
    public class SauceDemoTests : BaseTest
    {
        [Test]
        public void CompletePurchase()
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("        SAUCE DEMO AUTOMATION TEST");
            Console.WriteLine("==========================================");

            var saucePage = new SauceDemoPage(Driver);

            // ==================================================
            // 1. LOGIN
            // ==================================================

            Console.WriteLine();
            Console.WriteLine("[1/6] LOGIN");
            Console.WriteLine("----------------------------------------------");

            saucePage.NavigateTo();
            saucePage.LoginWithConfiguredCredentials();

            Assert.That(
                saucePage.IsProductsPageDisplayed(),
                Is.True,
                "Login failed - Products page was not displayed."
            );

            Console.WriteLine("PASS - Login successful.");

            // ==================================================
            // 2. ADD THREE PRODUCTS
            // ==================================================

            Console.WriteLine();
            Console.WriteLine("[2/6] ADD 3 PRODUCTS");
            Console.WriteLine("----------------------------------------------");

            var products = new List<string>
            {
                "sauce-labs-backpack",
                "sauce-labs-bike-light",
                "sauce-labs-bolt-t-shirt"
            };

            saucePage.AddMultipleProducts(products);

            var cartCount = saucePage.GetCartCount();

            Assert.That(
                cartCount,
                Is.EqualTo(3),
                "Expected 3 products in the cart."
            );

            Console.WriteLine("PASS - Cart badge shows 3 products.");

            // ==================================================
            // 3. OPEN CART AND VERIFY PRODUCTS
            // ==================================================

            Console.WriteLine();
            Console.WriteLine("[3/6] VERIFY SHOPPING CART");
            Console.WriteLine("----------------------------------------------");

            saucePage.OpenCart();

            Assert.Multiple(() =>
            {
                Assert.That(
                    saucePage.IsProductInCart("Sauce Labs Backpack"),
                    Is.True,
                    "Sauce Labs Backpack is missing from the cart."
                );

                Assert.That(
                    saucePage.IsProductInCart("Sauce Labs Bike Light"),
                    Is.True,
                    "Sauce Labs Bike Light is missing from the cart."
                );

                Assert.That(
                    saucePage.IsProductInCart("Sauce Labs Bolt T-Shirt"),
                    Is.True,
                    "Sauce Labs Bolt T-Shirt is missing from the cart."
                );
            });

            Console.WriteLine("PASS - All 3 products verified in cart.");

            // ==================================================
            // 4. REMOVE ONE PRODUCT
            // ==================================================

            Console.WriteLine();
            Console.WriteLine("[4/6] REMOVE 1 PRODUCT");
            Console.WriteLine("----------------------------------------------");

            const string removedProductId = "sauce-labs-backpack";
            const string removedProductName = "Sauce Labs Backpack";

            Console.WriteLine($"Removing: {removedProductName}");

            saucePage.RemoveProductFromCart(removedProductId);

            cartCount = saucePage.GetCartCount();

            Assert.That(
                cartCount,
                Is.EqualTo(2),
                "Expected 2 products after removing one."
            );

            Assert.Multiple(() =>
            {
                Assert.That(
                    saucePage.IsProductInCart(removedProductName),
                    Is.False,
                    $"{removedProductName} was not removed from the cart."
                );

                Assert.That(
                    saucePage.IsProductInCart("Sauce Labs Bike Light"),
                    Is.True,
                    "Sauce Labs Bike Light should remain in the cart."
                );

                Assert.That(
                    saucePage.IsProductInCart("Sauce Labs Bolt T-Shirt"),
                    Is.True,
                    "Sauce Labs Bolt T-Shirt should remain in the cart."
                );
            });

            Console.WriteLine("PASS - Backpack successfully removed.");
            Console.WriteLine("PASS - Remaining products are still in the cart.");

            // ==================================================
            // 5. CHECKOUT
            // ==================================================

            Console.WriteLine();
            Console.WriteLine("[5/6] CHECKOUT");
            Console.WriteLine("----------------------------------------------");

            saucePage.Checkout(
                "Boitumelo",
                "Khauoe",
                "1448"
            );

            Assert.Multiple(() =>
            {
                Assert.That(
                    saucePage.IsProductInCart("Sauce Labs Bike Light"),
                    Is.True,
                    "Sauce Labs Bike Light is missing from checkout."
                );

                Assert.That(
                    saucePage.IsProductInCart("Sauce Labs Bolt T-Shirt"),
                    Is.True,
                    "Sauce Labs Bolt T-Shirt is missing from checkout."
                );

                Assert.That(
                    saucePage.IsProductInCart("Sauce Labs Backpack"),
                    Is.False,
                    "Sauce Labs Backpack should not be present at checkout."
                );
            });

            Console.WriteLine("PASS - Checkout contains the correct products.");

            // ==================================================
            // 6. FINISH ORDER
            // ==================================================

            Console.WriteLine();
            Console.WriteLine("[6/6] VERIFY ORDER COMPLETION");
            Console.WriteLine("----------------------------------------------");

            saucePage.FinishOrder();

            var confirmation = saucePage.GetOrderConfirmation();

            Assert.That(
                confirmation,
                Is.EqualTo("Thank you for your order!"),
                "Order completion message was not displayed."
            );

            Console.WriteLine("PASS - Order completed successfully.");

            Console.WriteLine();
            Console.WriteLine("==============================================");
            Console.WriteLine("          SAUCE DEMO TEST PASSED");
            Console.WriteLine("==============================================");
        }
    }
}
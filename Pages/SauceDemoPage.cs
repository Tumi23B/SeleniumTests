using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace SeleniumTests.Pages
{
    public class SauceDemoPage : BasePage
    {
        // ==========================================================
        // LOGIN LOCATORS
        // ==========================================================

        private static readonly By UsernameField =
            By.Id("user-name");

        private static readonly By PasswordField =
            By.Id("password");

        private static readonly By LoginButton =
            By.Id("login-button");

        // ==========================================================
        // PRODUCTS PAGE LOCATORS
        // ==========================================================

        private static readonly By PageTitle =
            By.ClassName("title");

        private static readonly By CartBadge =
            By.ClassName("shopping_cart_badge");

        private static readonly By CartLink =
            By.ClassName("shopping_cart_link");

        // ==========================================================
        // CART LOCATORS
        // ==========================================================

        private static readonly By CartList =
            By.ClassName("cart_list");

        // ==========================================================
        // CHECKOUT LOCATORS
        // ==========================================================

        private static readonly By CheckoutButton =
            By.Id("checkout");

        private static readonly By FirstNameField =
            By.Id("first-name");

        private static readonly By LastNameField =
            By.Id("last-name");

        private static readonly By PostalCodeField =
            By.Id("postal-code");

        private static readonly By ContinueButton =
            By.Id("continue");

        private static readonly By FinishButton =
            By.Id("finish");

        // ==========================================================
        // ORDER CONFIRMATION
        // ==========================================================

        private static readonly By CompleteHeader =
            By.ClassName("complete-header");

        // ==========================================================
        // TEST DATA
        // ==========================================================

        private const string ProductsPageTitle =
            "Products";

        private const string OrderConfirmationMessage =
            "Thank you for your order!";

        // ==========================================================
        // CONSTRUCTOR
        // ==========================================================

        public SauceDemoPage(IWebDriver driver)
            : base(driver)
        {
        }

        // ==========================================================
        // NAVIGATION
        // ==========================================================

        
        // Navigates to the configured SauceDemo application.
        
        public void NavigateTo()
        {
            NavigateTo(
                Config.Urls.SauceDemo.Base
            );

            Console.WriteLine(
                $"Navigated to: {Config.Urls.SauceDemo.Base}"
            );
        }

        // ==========================================================
        // LOGIN
        // ==========================================================

      
        // Logs into SauceDemo using the supplied credentials.
       
        public void Login(
            string username,
            string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException(
                    "Username cannot be null or empty.",
                    nameof(username)
                );
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(
                    "Password cannot be null or empty.",
                    nameof(password)
                );
            }

            Type(
                UsernameField,
                username
            );

            Type(
                PasswordField,
                password
            );

            Click(LoginButton);

            WaitForElementVisible(PageTitle);

            Console.WriteLine(
                "SauceDemo login completed successfully."
            );
        }

        
        // Logs into SauceDemo using the credentials configured in the local configuration file.
        
        public void LoginWithConfiguredCredentials()
        {
            Login(
                Config.Credentials.Login.ValidUsername,
                Config.Credentials.Login.ValidPassword
            );
        }

        // ==========================================================
        // PRODUCTS PAGE
        // ==========================================================

        
        // Verifies that the Products page is displayed.
      
        public bool IsProductsPageDisplayed()
        {
            try
            {
                var title = GetText(PageTitle);

                return title.Equals(
                    ProductsPageTitle,
                    StringComparison.OrdinalIgnoreCase
                );
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }

        // ==========================================================
        // CART - ADD PRODUCTS
        // ==========================================================

        
        // Adds a product to the shopping cart using its product ID.
        
        public void AddProductToCart(
            string productId)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new ArgumentException(
                    "Product ID cannot be null or empty.",
                    nameof(productId)
                );
            }

            var addButton = By.Id(
                $"add-to-cart-{productId}"
            );

            var removeButton = By.Id(
                $"remove-{productId}"
            );

            Click(addButton);

            // The remove button appears after the product has successfully been added.
            WaitForElementVisible(
                removeButton
            );

            Console.WriteLine(
                $"PASS - {productId} added to cart."
            );
        }

      
        // Adds multiple products to the shopping cart.

        public void AddMultipleProducts(
            IEnumerable<string> productIds)
        {
            if (productIds == null)
            {
                throw new ArgumentNullException(
                    nameof(productIds)
                );
            }

            foreach (var productId in productIds)
            {
                AddProductToCart(
                    productId
                );

                PauseForVisual(
                    $"Added {productId}"
                );
            }
        }

        // ==========================================================
        // CART - COUNT
        // ==========================================================

     
        // Gets the current number of items in the cart.
        // Returns zero when the cart badge is not displayed.
      
        public int GetCartCount()
        {
            try
            {
                var badge = WaitForElementVisible(
                    CartBadge
                );

                return int.TryParse(
                    badge.Text.Trim(),
                    out var count
                )
                    ? count
                    : 0;
            }
            catch (WebDriverTimeoutException)
            {
                return 0;
            }
            catch (NoSuchElementException)
            {
                return 0;
            }
        }

        // ==========================================================
        // CART - OPEN
        // ==========================================================

        
        // Opens the shopping cart.
       
        public void OpenCart()
        {
            Click(CartLink);

            WaitForElementVisible(
                CartList
            );

            Console.WriteLine(
                "Shopping cart opened."
            );
        }

        // ==========================================================
        // CART - ITEMS
        // ==========================================================

        
        // Gets the visible text from the shopping cart.
        
        public List<string> GetCartItems()
        {
            var cartText = GetText(
                CartList
            );

            return cartText
                .Split(
                    '\n',
                    StringSplitOptions.RemoveEmptyEntries
                )
                .Select(
                    item => item.Trim()
                )
                .Where(
                    item => !string.IsNullOrWhiteSpace(item)
                )
                .ToList();
        }

        
        // Determines whether a specific product exists in the cart.
       
        public bool IsProductInCart(
            string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                return false;
            }

            var items = GetCartItems();

            return items.Any(
                item => item.Contains(
                    productName,
                    StringComparison.OrdinalIgnoreCase
                )
            );
        }

        // ==========================================================
        // CART - REMOVE PRODUCTS
        // ==========================================================

       
        // Removes a product from the shopping cart using its product ID.
        
        public void RemoveProductFromCart(
            string productId)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new ArgumentException(
                    "Product ID cannot be null or empty.",
                    nameof(productId)
                );
            }

            var removeButton = By.Id(
                $"remove-{productId}"
            );

            Click(removeButton);

            Console.WriteLine(
                $"Product removed from cart: {productId}"
            );

            PauseForVisual(
                $"Removed {productId}"
            );
        }

        // ==========================================================
        // CHECKOUT
        // ==========================================================

        
        // Completes the checkout information step.
    
        public void Checkout(
            string firstName,
            string lastName,
            string postalCode)
        {
            ValidateCheckoutInformation(
                firstName,
                lastName,
                postalCode
            );

            Click(
                CheckoutButton
            );

            WaitForElementVisible(
                FirstNameField
            );

            Type(
                FirstNameField,
                firstName
            );

            Type(
                LastNameField,
                lastName
            );

            Type(
                PostalCodeField,
                postalCode
            );

            Click(
                ContinueButton
            );

            WaitForElementVisible(
                FinishButton
            );

            Console.WriteLine(
                "Checkout information submitted."
            );
        }

        // ==========================================================
        // ORDER COMPLETION
        // ==========================================================

        
        // Completes the order.
        
        public void FinishOrder()
        {
            Click(
                FinishButton
            );

            WaitForElementVisible(
                CompleteHeader
            );

            Console.WriteLine(
                "Order completed successfully."
            );

            PauseForVisual(
                "Order complete"
            );
        }

        
        // Gets the order confirmation message.
        
        public string GetOrderConfirmation()
        {
            return GetText(
                CompleteHeader
            );
        }

        
        // Verifies that the expected order confirmation message is displayed.
      
        public bool IsOrderCompleted()
        {
            try
            {
                var confirmation =
                    GetOrderConfirmation();

                return confirmation.Equals(
                    OrderConfirmationMessage,
                    StringComparison.OrdinalIgnoreCase
                );
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }

        // ==========================================================
        // PRIVATE VALIDATION
        // ==========================================================

       
        // Validates the checkout information before interacting/moving on  with the checkout form.
        
        private static void ValidateCheckoutInformation(
            string firstName,
            string lastName,
            string postalCode)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException(
                    "First name cannot be null or empty.",
                    nameof(firstName)
                );
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException(
                    "Last name cannot be null or empty.",
                    nameof(lastName)
                );
            }

            if (string.IsNullOrWhiteSpace(postalCode))
            {
                throw new ArgumentException(
                    "Postal code cannot be null or empty.",
                    nameof(postalCode)
                );
            }
        }
    }
}
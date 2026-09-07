using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.Tests
{
    [TestFixture]
    public class DropdownTests : BaseTest
    {
        private static readonly By DropdownLocator =
            By.Id("dropdown");

        [Test]
        public void TestDropdown()
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("TEST: Dropdown Operations");
            Console.WriteLine("==========================================");

            // --------------------------------------------------
            // Step 1: Navigate to dropdown page
            // --------------------------------------------------

            Driver.Navigate().GoToUrl(
                Config.Urls.TheInternet.Dropdown
            );

            Console.WriteLine(
                $"Navigated to: {Config.Urls.TheInternet.Dropdown}"
            );

            // --------------------------------------------------
            // Step 2: Wait for dropdown to be available
            // --------------------------------------------------

            var wait = new WebDriverWait(
                Driver,
                TimeSpan.FromSeconds(
                    Config.Selenium.DefaultTimeout
                )
            );

            var dropdownElement = wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(
                        DropdownLocator
                    );

                    return element.Displayed && element.Enabled
                        ? element
                        : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });

            Assert.That(
                dropdownElement,
                Is.Not.Null,
                "Dropdown should be displayed and enabled."
            );

            var dropdown = new SelectElement(
                dropdownElement!
            );

            Console.WriteLine(
                "Dropdown is displayed and ready."
            );

            // --------------------------------------------------
            // Step 3: Select Option 1 by visible text
            // --------------------------------------------------

            Console.WriteLine(
                "Selecting Option 1 by visible text..."
            );

            dropdown.SelectByText("Option 1");

            Assert.That(
                dropdown.SelectedOption.Text,
                Is.EqualTo("Option 1"),
                "Option 1 should be selected."
            );

            Console.WriteLine(
                "PASS - Option 1 selected successfully."
            );

            // --------------------------------------------------
            // Step 4: Select Option 2 by value
            // --------------------------------------------------

            Console.WriteLine(
                "Selecting Option 2 by value..."
            );

            dropdown.SelectByValue("2");

            Assert.That(
                dropdown.SelectedOption.Text,
                Is.EqualTo("Option 2"),
                "Option 2 should be selected."
            );

            Console.WriteLine(
                "PASS - Option 2 selected successfully."
            );

            // --------------------------------------------------
            // Step 5: Select Option 1 by index
            // --------------------------------------------------

            Console.WriteLine(
                "Selecting Option 1 by index..."
            );

            dropdown.SelectByIndex(1);

            Assert.That(
                dropdown.SelectedOption.Text,
                Is.EqualTo("Option 1"),
                "Index 1 should select Option 1."
            );

            Console.WriteLine(
                $"Selected option by index: {dropdown.SelectedOption.Text}"
            );

            // --------------------------------------------------
            // Test completed
            // --------------------------------------------------

            Console.WriteLine(
                "PASS - Dropdown test completed successfully."
            );
        }
    }
}
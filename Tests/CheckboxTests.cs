using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.Tests
{
    [TestFixture]
    public class CheckboxTests : BaseTest
    {
        private const string CheckboxSelector =
            "input[type='checkbox']";

        [Test]
        public void TestCheckboxes()
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("TEST: Checkbox Operations");
            Console.WriteLine("==========================================");

            // --------------------------------------------------
            // Navigate to the checkbox page
            // --------------------------------------------------

            Driver.Navigate().GoToUrl(
                Config.Urls.TheInternet.Checkboxes
            );

            Console.WriteLine(
                $"Navigated to: {Config.Urls.TheInternet.Checkboxes}"
            );

            // --------------------------------------------------
            // Wait for both checkboxes to be available
            // --------------------------------------------------

            var wait = new WebDriverWait(
                Driver,
                TimeSpan.FromSeconds(
                    Config.Selenium.DefaultTimeout
                )
            );

            var checkboxes = wait.Until(driver =>
            {
                var elements = driver
                    .FindElements(
                        By.CssSelector(CheckboxSelector)
                    )
                    .ToList();

                return elements.Count >= 2
                    ? elements
                    : null;
            });

            Assert.That(
                checkboxes,
                Is.Not.Null,
                "Checkbox elements should be present."
            );

            Assert.That(
                checkboxes!.Count,
                Is.GreaterThanOrEqualTo(2),
                "Expected at least two checkboxes."
            );

            var checkbox1 = checkboxes[0];
            var checkbox2 = checkboxes[1];

            // --------------------------------------------------
            // Verify initial state
            // --------------------------------------------------

            Console.WriteLine(
                $"Initial Checkbox 1 selected: {checkbox1.Selected}"
            );

            Console.WriteLine(
                $"Initial Checkbox 2 selected: {checkbox2.Selected}"
            );

            // --------------------------------------------------
            // Select both checkboxes
            // --------------------------------------------------

            if (!checkbox1.Selected)
            {
                checkbox1.Click();
            }

            if (!checkbox2.Selected)
            {
                checkbox2.Click();
            }

            Assert.Multiple(() =>
            {
                Assert.That(
                    checkbox1.Selected,
                    Is.True,
                    "Checkbox 1 should be selected."
                );

                Assert.That(
                    checkbox2.Selected,
                    Is.True,
                    "Checkbox 2 should be selected."
                );
            });

            Console.WriteLine(
                "PASS - Both checkboxes were selected."
            );

            // --------------------------------------------------
            // Unselect both checkboxes
            // --------------------------------------------------

            if (checkbox1.Selected)
            {
                checkbox1.Click();
            }

            if (checkbox2.Selected)
            {
                checkbox2.Click();
            }

            // --------------------------------------------------
            // Verify both checkboxes are unchecked
            // --------------------------------------------------

            Assert.Multiple(() =>
            {
                Assert.That(
                    checkbox1.Selected,
                    Is.False,
                    "Checkbox 1 should be unchecked."
                );

                Assert.That(
                    checkbox2.Selected,
                    Is.False,
                    "Checkbox 2 should be unchecked."
                );
            });

            Console.WriteLine(
                "PASS - Both checkboxes are unchecked."
            );
        }
    }
}
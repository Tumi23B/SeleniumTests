using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumTests.Config;

namespace SeleniumTests.Pages
{
    public abstract class BasePage
    {
        protected IWebDriver Driver { get; }
        protected WebDriverWait Wait { get; }
        protected AppConfig Config { get; }

        protected BasePage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));

            Config = AppConfig.Instance;

            Wait = new WebDriverWait(
                Driver,
                TimeSpan.FromSeconds(Config.Selenium.DefaultTimeout)
            );
        }

        // Wait until an element exists in the DOM.
        protected IWebElement WaitForElement(By locator)
        {
            return Wait.Until(driver =>
            {
                try
                {
                    return driver.FindElement(locator);
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            })!;
        }

        // Wait until an element is visible.
        protected IWebElement WaitForElementVisible(By locator)
        {
            return Wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(locator);
                    return element.Displayed ? element : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            })!;
        }

        // Wait until an element is visible and enabled.
        protected IWebElement WaitForElementClickable(By locator)
        {
            return Wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(locator);

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
            })!;
        }

        protected void Click(By locator)
        {
            var element = WaitForElementClickable(locator);

            ScrollToElement(element);

            element.Click();
        }

        protected void Type(By locator, string text)
        {
            var element = WaitForElementVisible(locator);

            element.Clear();
            element.SendKeys(text);
        }

        protected string GetText(By locator)
        {
            return WaitForElementVisible(locator).Text;
        }

        protected bool IsDisplayed(By locator)
        {
            try
            {
                return Driver.FindElement(locator).Displayed;
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

        protected void ScrollToElement(IWebElement element)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript(
                "arguments[0].scrollIntoView({behavior:'instant', block:'center'});",
                element
            );
        }

        protected void ScrollToTop()
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript(
                "window.scrollTo(0, 0);"
            );
        }

        protected void ScrollToBottom()
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript(
                "window.scrollTo(0, document.body.scrollHeight);"
            );
        }

        protected void WaitForUrlContains(string text)
        {
            Wait.Until(driver =>
                driver.Url.Contains(text, StringComparison.OrdinalIgnoreCase)
            );
        }

        protected void WaitForTitleContains(string text)
        {
            Wait.Until(driver =>
                driver.Title.Contains(text, StringComparison.OrdinalIgnoreCase)
            );
        }

        protected void NavigateTo(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        protected void PauseForVisual(string action)
        {
            Console.WriteLine($"Pausing to view: {action}");

            if (Config.Selenium.VisualPause > 0)
            {
                System.Threading.Thread.Sleep(Config.Selenium.VisualPause);
            }
        }
    }
}
using System;
using OpenQA.Selenium;

namespace SeleniumTests.Pages
{
    public class LoginPage : BasePage
    {
        // ==========================================================
        // LOCATORS
        // ==========================================================

        private readonly By _usernameField =
            By.Id("username");

        private readonly By _passwordField =
            By.Id("password");

        private readonly By _loginButton =
            By.CssSelector("button[type='submit']");

        private readonly By _logoutButton =
            By.CssSelector("a[href='/logout']");

        private readonly By _flashMessage =
            By.Id("flash");

        // ==========================================================
        // CONSTRUCTOR
        // ==========================================================

        public LoginPage(IWebDriver driver)
            : base(driver)
        {
        }

        // ==========================================================
        // NAVIGATION
        // ==========================================================
        //  Navigates to the configured login page.

  
        
        public void NavigateTo()
        {
            NavigateTo(Config.Urls.TheInternet.Login);

            Console.WriteLine(
                $"Navigated to: {Config.Urls.TheInternet.Login}"
            );
        }

        // ==========================================================
        // LOGIN
        // ==========================================================

        
        // Logs in using the supplied credentials.
        // Credentials are never written to the console.
        
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
                _usernameField,
                username
            );

            Type(
                _passwordField,
                password
            );

            Click(_loginButton);

            Console.WriteLine(
                "Login action submitted."
            );
        }

        
        // Logs in using the valid credentials
        // configured in appsettings.local.json.
        // 
        public void LoginWithValidCredentials()
        {
            Login(
                Config.Credentials.Login.ValidUsername,
                Config.Credentials.Login.ValidPassword
            );
        }

        
        // Logs in using the configured invalid username
        // and invalid password.
        
        public void LoginWithInvalidCredentials()
        {
            Login(
                Config.Credentials.Login.InvalidUsername,
                Config.Credentials.Login.InvalidPassword
            );
        }

        // Logs in using the configured valid username
        // and invalid password.
        
        public void LoginWithInvalidPassword()
        {
            Login(
                Config.Credentials.Login.ValidUsername,
                Config.Credentials.Login.InvalidPassword
            );
        }

      
        // Logs in using an invalid username and
        // the configured valid password.
       
        public void LoginWithInvalidUsername()
        {
            Login(
                Config.Credentials.Login.InvalidUsername,
                Config.Credentials.Login.ValidPassword
            );
        }

        // ==========================================================
        // LOGOUT
        // ==========================================================

        
        // Logs out of the application.
        
        public void Logout()
        {
            Click(_logoutButton);

            Console.WriteLine(
                "Logout action completed."
            );
        }

        // ==========================================================
        // MESSAGES
        // ==========================================================

        
        // Gets the currently displayed flash message.
        
        public string GetFlashMessage()
        {
            return GetText(_flashMessage);
        }

    
        // Verifies that the configured login success message is displayed.
       
        public bool IsLoginSuccessful()
        {
            return IsMessageDisplayed(
                Config.Messages.LoginSuccess
            );
        }

        
        /// Verifies that the configured logout success message is displayed.
        
        public bool IsLogoutSuccessful()
        {
            return IsMessageDisplayed(
                Config.Messages.LogoutSuccess
            );
        }

        
        // Verifies that the expected error message is displayed.

        public bool IsErrorMessageDisplayed(
            string expectedMessage)
        {
            if (string.IsNullOrWhiteSpace(expectedMessage))
            {
                return false;
            }

            return IsMessageDisplayed(
                expectedMessage
            );
        }

        // ==========================================================
        // PRIVATE HELPERS
        // ==========================================================

      
        // Checks whether the flash message contains the expected text.
        
        private bool IsMessageDisplayed(
            string expectedMessage)
        {
            if (string.IsNullOrWhiteSpace(expectedMessage))
            {
                return false;
            }

            try
            {
                var actualMessage =
                    GetFlashMessage();

                return actualMessage.Contains(
                    expectedMessage,
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
    }
}
using NUnit.Framework;
using SeleniumTests.Pages;

namespace SeleniumTests.Tests
{
    [TestFixture]
    public class LoginTests : BaseTest
    {
        // ==========================================================
        // VALID LOGIN AND LOGOUT
        // ==========================================================

        [Test]
        public void LoginAndLogoutWithValidCredentials()
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("TEST: Login and Logout with Valid Credentials");
            Console.WriteLine("==========================================");

            // --------------------------------------------------
            // Step 1: Create login page
            // --------------------------------------------------

            var loginPage = new LoginPage(Driver);

            // --------------------------------------------------
            // Step 2: Navigate to login page
            // --------------------------------------------------

            loginPage.NavigateTo();

            // --------------------------------------------------
            // Step 3: Login with valid credentials
            // --------------------------------------------------

            loginPage.LoginWithValidCredentials();

            // --------------------------------------------------
            // Step 4: Verify successful login
            // --------------------------------------------------

            Assert.That(
                loginPage.IsLoginSuccessful(),
                Is.True,
                $"Expected login success message containing: " +
                $"'{Config.Messages.LoginSuccess}'"
            );

            Console.WriteLine(
                "PASS - Login successful."
            );

            // --------------------------------------------------
            // Step 5: Logout
            // --------------------------------------------------

            loginPage.Logout();

            // --------------------------------------------------
            // Step 6: Verify successful logout
            // --------------------------------------------------

            Assert.That(
                loginPage.IsLogoutSuccessful(),
                Is.True,
                $"Expected logout success message containing: " +
                $"'{Config.Messages.LogoutSuccess}'"
            );

            Console.WriteLine(
                "PASS - Logout successful."
            );

            Console.WriteLine(
                "PASS - Valid login and logout completed successfully."
            );
        }

        // ==========================================================
        // INVALID CREDENTIALS
        // ==========================================================

        [Test]
        public void LoginWithIncorrectCredentials()
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("TEST: Login with Incorrect Credentials");
            Console.WriteLine("==========================================");

            // --------------------------------------------------
            // Step 1: Create login page
            // --------------------------------------------------

            var loginPage = new LoginPage(Driver);

            // --------------------------------------------------
            // Step 2: Navigate to login page
            // --------------------------------------------------

            loginPage.NavigateTo();

            // --------------------------------------------------
            // Step 3: Attempt login with invalid credentials
            // --------------------------------------------------

            loginPage.LoginWithInvalidCredentials();

            // --------------------------------------------------
            // Step 4: Verify error message
            // --------------------------------------------------

            Assert.That(
                loginPage.IsErrorMessageDisplayed(
                    Config.Messages.LoginError
                ),
                Is.True,
                $"Expected error message containing: " +
                $"'{Config.Messages.LoginError}'"
            );

            Console.WriteLine(
                "PASS - Invalid login error message displayed correctly."
            );
        }

        // ==========================================================
        // EMPTY CREDENTIALS
        // ==========================================================

        [Test]
        public void LoginWithEmptyCredentials()
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("TEST: Login with Empty Credentials");
            Console.WriteLine("==========================================");

            // --------------------------------------------------
            // Step 1: Create login page
            // --------------------------------------------------

            var loginPage = new LoginPage(Driver);

            // --------------------------------------------------
            // Step 2: Navigate to login page
            // --------------------------------------------------

            loginPage.NavigateTo();

            // --------------------------------------------------
            // Step 3: Attempt login with empty credentials
            // --------------------------------------------------

            loginPage.Login(
                string.Empty,
                string.Empty
            );

            // --------------------------------------------------
            // Step 4: Verify validation message
            // --------------------------------------------------

            Assert.That(
                loginPage.IsErrorMessageDisplayed(
                    Config.Messages.LoginError
                ),
                Is.True,
                $"Expected error message containing: " +
                $"'{Config.Messages.LoginError}'"
            );

            Console.WriteLine(
                "PASS - Empty credentials handled correctly."
            );
        }

        // ==========================================================
        // VALID USERNAME / INVALID PASSWORD
        // ==========================================================

        [Test]
        public void LoginWithValidUsernameInvalidPassword()
        {
            Console.WriteLine("==========================================");
            Console.WriteLine(
                "TEST: Login with Valid Username, Invalid Password"
            );
            Console.WriteLine("==========================================");

            // --------------------------------------------------
            // Step 1: Create login page
            // --------------------------------------------------

            var loginPage = new LoginPage(Driver);

            // --------------------------------------------------
            // Step 2: Navigate to login page
            // --------------------------------------------------

            loginPage.NavigateTo();

            // --------------------------------------------------
            // Step 3: Attempt login with invalid password
            // --------------------------------------------------

            loginPage.LoginWithInvalidPassword();

            // --------------------------------------------------
            // Step 4: Verify password error
            // --------------------------------------------------

            Assert.That(
                loginPage.IsErrorMessageDisplayed(
                    Config.Messages.PasswordError
                ),
                Is.True,
                $"Expected password error message containing: " +
                $"'{Config.Messages.PasswordError}'"
            );

            Console.WriteLine(
                "PASS - Invalid password handled correctly."
            );
        }
    }
}
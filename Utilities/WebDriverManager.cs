using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests.Utilities
{
    public class WebDriverManager : IDisposable
    {
        private IWebDriver? _driver;
        private bool _disposed;

        public WebDriverManager(bool headless = false)
        {
            InitializeDriver(headless);
        }

        // ============================================================
        // DRIVER INITIALIZATION
        // ============================================================

        private void InitializeDriver(bool headless)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(
                    nameof(WebDriverManager)
                );
            }

            var options = new ChromeOptions();

            // --------------------------------------------------------
            // Browser configuration
            // --------------------------------------------------------

            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            // --------------------------------------------------------
            // Disable Chrome password manager prompts
            // --------------------------------------------------------

            options.AddUserProfilePreference(
                "credentials_enable_service",
                false
            );

            options.AddUserProfilePreference(
                "profile.password_manager_enabled",
                false
            );

            options.AddUserProfilePreference(
                "profile.password_manager_leak_detection",
                false
            );

            // --------------------------------------------------------
            // Configure download directory
            // --------------------------------------------------------

            var downloadPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Downloads"
            );

            Directory.CreateDirectory(downloadPath);

            options.AddUserProfilePreference(
                "download.default_directory",
                downloadPath
            );

            options.AddUserProfilePreference(
                "download.prompt_for_download",
                false
            );

            options.AddUserProfilePreference(
                "download.directory_upgrade",
                true
            );

            // --------------------------------------------------------
            // Headless mode
            // --------------------------------------------------------

            if (headless)
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--window-size=1920,1080");
            }

            // --------------------------------------------------------
            // Start Chrome
            // --------------------------------------------------------

            Console.WriteLine(
                "Starting Chrome WebDriver..."
            );

            // Selenium Manager automatically locates the
            // compatible ChromeDriver.
            _driver = new ChromeDriver(options);

            Console.WriteLine(
                "Chrome WebDriver started successfully."
            );
        }

        // ============================================================
        // GET DRIVER
        // ============================================================

        public IWebDriver GetDriver()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(
                    nameof(WebDriverManager)
                );
            }

            if (_driver == null)
            {
                throw new InvalidOperationException(
                    "WebDriver has not been initialized."
                );
            }

            return _driver;
        }

        // ============================================================
        // DISPOSE MANAGER
        // ============================================================
        //
        // NB:
        // WebDriverManager does NOT dispose the IWebDriver here.
        //
        // BaseTest owns the IWebDriver and disposes it explicitly inside [TearDown]. 
        // This satisfies NUnit1032 and prevents duplicate ownership of the same WebDriver instance.
        

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            Console.WriteLine(
                "Disposing WebDriverManager..."
            );

            _driver = null;

            _disposed = true;

            Console.WriteLine(
                "WebDriverManager disposed successfully."
            );

            GC.SuppressFinalize(this);
        }
    }
}
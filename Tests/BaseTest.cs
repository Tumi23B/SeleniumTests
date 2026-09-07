using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using SeleniumTests.Config;
using SeleniumTests.Utilities;

namespace SeleniumTests.Tests
{
    public abstract class BaseTest
    {
        protected IWebDriver Driver { get; private set; } = null!;

        protected ScreenshotHelper? ScreenshotHelper { get; private set; }

        protected AppConfig Config { get; private set; } = null!;

        protected ReportHelper Report =>
            ReportHelper.Instance;

        protected WebDriverManager? DriverManager { get; private set; }

        private DateTime _testStartTime;

        // ============================================================
        // TEST SETUP
        // ============================================================

        [SetUp]
        public void SetUp()
        {
            try
            {
                Config = AppConfig.Instance;

                _testStartTime = DateTime.Now;

                Console.WriteLine();
                Console.WriteLine("==========================================");
                Console.WriteLine(
                    $"STARTING TEST: {TestContext.CurrentContext.Test.Name}"
                );
                Console.WriteLine(
                    $"START TIME: {_testStartTime:yyyy-MM-dd HH:mm:ss}"
                );
                Console.WriteLine("==========================================");

                // ----------------------------------------------------
                // CREATE WEBDRIVER MANAGER
                // ----------------------------------------------------

                DriverManager =
                    new WebDriverManager(
                        Config.Selenium.Headless
                    );

                // ----------------------------------------------------
                // GET WEBDRIVER
                // ----------------------------------------------------

                Driver =
                    DriverManager.GetDriver();

                // ----------------------------------------------------
                // CREATE SCREENSHOT HELPER
                // ----------------------------------------------------

                ScreenshotHelper =
                    new ScreenshotHelper(Driver);

                Console.WriteLine(
                    "WebDriver initialized successfully."
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(
                    $"FAILED TO INITIALIZE TEST: {ex.Message}"
                );

                CleanupAfterSetupFailure();

                throw;
            }
        }

        // ============================================================
        // TEST TEARDOWN
        // ============================================================

        [TearDown]
        public void TearDown()
        {
            var testContext =
                TestContext.CurrentContext;

            var testEndTime =
                DateTime.Now;

            var duration =
                testEndTime - _testStartTime;

            var testName =
                testContext.Test.Name;

            var testStatus =
                testContext.Result.Outcome.Status;

            var passed =
                testStatus == TestStatus.Passed;

            var errorMessage =
                testContext.Result.Message ?? string.Empty;

            Console.WriteLine();
            Console.WriteLine("------------------------------------------");
            Console.WriteLine(
                $"FINISHED TEST: {testName}"
            );
            Console.WriteLine(
                $"END TIME: {testEndTime:yyyy-MM-dd HH:mm:ss}"
            );
            Console.WriteLine(
                $"TEST DURATION: {duration.TotalSeconds:F2}s"
            );
            Console.WriteLine(
                $"TEST RESULT: {testStatus}"
            );
            Console.WriteLine("------------------------------------------");

            // ========================================================
            // TAKE SCREENSHOT FOR EVERY TEST
            // ========================================================

            var screenshotPath = string.Empty;

            try
            {
                if (ScreenshotHelper != null)
                {
                    Console.WriteLine(
                        $"Capturing screenshot: {testName}"
                    );

                    screenshotPath =
                        ScreenshotHelper.TakeScreenshot(
                            testName
                        );

                    if (!string.IsNullOrWhiteSpace(screenshotPath))
                    {
                        Console.WriteLine(
                            $"Screenshot saved: {screenshotPath}"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Unable to capture screenshot: {ex.Message}"
                );
            }

            // ========================================================
            // RECORD TEST RESULT
            // ========================================================

            try
            {
                Report.AddResult(
                    new TestResult
                    {
                        TestName = testName,
                        Passed = passed,
                        ErrorMessage = errorMessage,
                        ScreenshotPath = screenshotPath,
                        Timestamp = _testStartTime,
                        Duration = duration
                    }
                );

                Console.WriteLine(
                    "Test result added to report."
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Unable to record test result: {ex.Message}"
                );
            }

            // ========================================================
            // DISPOSE WEBDRIVER
            // ========================================================
            
            // NB:
            // Driver is explicitly disposed inside [TearDown]
            // to satisfy NUnit1032.
            

            try
            {
                if (Driver != null)
                {
                    Console.WriteLine(
                        "Quitting WebDriver..."
                    );

                    Driver.Quit();

                    Console.WriteLine(
                        "WebDriver quit successfully."
                    );

                    Console.WriteLine(
                        "Disposing WebDriver..."
                    );

                    Driver.Dispose();

                    Console.WriteLine(
                        "WebDriver disposed successfully."
                    );
                }
            }
            catch (WebDriverException ex)
            {
                Console.WriteLine(
                    $"WebDriver cleanup warning: {ex.Message}"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Unexpected WebDriver cleanup error: {ex.Message}"
                );
            }

            // ========================================================
            // DISPOSE WEBDRIVER MANAGER
            // ========================================================
            
            // DriverManager is initialized in SetUp, therefore it is
            // explicitly disposed here inside [TearDown].
            

            try
            {
                if (DriverManager != null)
                {
                    Console.WriteLine(
                        "Disposing WebDriverManager..."
                    );

                    DriverManager.Dispose();

                    Console.WriteLine(
                        "WebDriverManager disposed successfully."
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"WebDriverManager cleanup warning: {ex.Message}"
                );
            }

            // ========================================================
            // CLEAR REFERENCES
            // ========================================================

            Driver = null!;

            ScreenshotHelper = null;

            DriverManager = null;

            Console.WriteLine(
                "WebDriver cleanup completed."
            );
        }

        // ============================================================
        // SETUP FAILURE CLEANUP
        // ============================================================

        private void CleanupAfterSetupFailure()
        {
            Console.WriteLine(
                "Cleaning up resources after setup failure..."
            );

            // --------------------------------------------------------
            // Dispose Driver
            // --------------------------------------------------------

            try
            {
                if (Driver != null)
                {
                    Driver.Quit();
                    Driver.Dispose();

                    Console.WriteLine(
                        "WebDriver disposed after setup failure."
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"WebDriver setup-failure cleanup warning: {ex.Message}"
                );
            }

            // --------------------------------------------------------
            // Dispose DriverManager
            // --------------------------------------------------------

            try
            {
                if (DriverManager != null)
                {
                    DriverManager.Dispose();

                    Console.WriteLine(
                        "WebDriverManager disposed after setup failure."
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"WebDriverManager setup-failure cleanup warning: {ex.Message}"
                );
            }

            Driver = null!;

            ScreenshotHelper = null;

            DriverManager = null;

            Console.WriteLine(
                "Setup-failure cleanup completed."
            );
        }
    }
}
using System;
using System.IO;
using OpenQA.Selenium;

namespace SeleniumTests.Utilities
{
    public class ScreenshotHelper
    {
        private readonly IWebDriver _driver;
        private readonly string _screenshotDirectory;

        public ScreenshotHelper(IWebDriver driver)
        {
            _driver =
                driver ?? throw new ArgumentNullException(nameof(driver));

            _screenshotDirectory =
                Config.AppConfig.Instance.Screenshots.Directory;

            EnsureScreenshotDirectory();
        }

        public string TakeScreenshot(string testName)
        {
            if (string.IsNullOrWhiteSpace(testName))
            {
                testName = "screenshot";
            }

            try
            {
                var timestamp =
                    DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");

                var safeTestName =
                    SanitizeFileName(testName);

                var filename =
                    $"{safeTestName}_{timestamp}.png";

                var screenshotPath =
                    Path.Combine(
                        _screenshotDirectory,
                        filename
                    );

                var screenshot =
                    ((ITakesScreenshot)_driver)
                    .GetScreenshot();

                screenshot.SaveAsFile(screenshotPath);

                var fullPath =
                    Path.GetFullPath(screenshotPath);

                Console.WriteLine(
                    $"Screenshot saved: {fullPath}"
                );

                return screenshotPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Failed to save screenshot: {ex.Message}"
                );

                return string.Empty;
            }
        }

        private void EnsureScreenshotDirectory()
        {
            try
            {
                Directory.CreateDirectory(
                    _screenshotDirectory
                );
            }
            catch (Exception ex)
            {
                throw new IOException(
                    $"Unable to create screenshot directory: " +
                    $"{_screenshotDirectory}",
                    ex
                );
            }
        }

        private static string SanitizeFileName(
            string fileName)
        {
            var invalidCharacters =
                Path.GetInvalidFileNameChars();

            foreach (var character in invalidCharacters)
            {
                fileName =
                    fileName.Replace(
                        character,
                        '_'
                    );
            }

            return fileName.Trim();
        }
    }
}
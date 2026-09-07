using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.Tests
{
    [TestFixture]
    public class FileDownloadTests : BaseTest
    {
        private const int DownloadTimeoutSeconds = 15;

        [Test]
        public void TestFileDownload()
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("TEST: File Download");
            Console.WriteLine("==========================================");

            // --------------------------------------------------
            // Step 1: Prepare download directory
            // --------------------------------------------------

            var downloadPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Downloads"
            );

            Directory.CreateDirectory(downloadPath);

            Console.WriteLine(
                $"Download directory: {downloadPath}"
            );

            // --------------------------------------------------
            // Step 2: Navigate to download page
            // --------------------------------------------------

            Driver.Navigate().GoToUrl(
                Config.Urls.TheInternet.FileDownload
            );

            Console.WriteLine(
                $"Navigated to: {Config.Urls.TheInternet.FileDownload}"
            );

            // --------------------------------------------------
            // Step 3: Wait for a valid download link
            // --------------------------------------------------

            var wait = new WebDriverWait(
                Driver,
                TimeSpan.FromSeconds(
                    Config.Selenium.DefaultTimeout
                )
            );

            var fileLink = wait.Until(driver =>
            {
                try
                {
                    var links = driver
                        .FindElements(
                            By.CssSelector("#content a")
                        );

                    return links.FirstOrDefault(link =>
                    {
                        var filename = link.Text.Trim();

                        return link.Displayed &&
                               link.Enabled &&
                               !string.IsNullOrWhiteSpace(filename) &&
                               !filename.StartsWith(
                                   "empty-",
                                   StringComparison.OrdinalIgnoreCase
                               );
                    });
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });

            Assert.That(
                fileLink,
                Is.Not.Null,
                "A non-empty file download link should be available."
            );

            // --------------------------------------------------
            // Step 4: Determine filename and file path
            // --------------------------------------------------

            var filename = fileLink!.Text.Trim();

            Assert.That(
                filename,
                Is.Not.Empty,
                "The download link should contain a filename."
            );

            Console.WriteLine(
                $"File selected for download: {filename}"
            );

            var filePath = Path.Combine(
                downloadPath,
                filename
            );

            // --------------------------------------------------
            // Step 5: Remove previous copy if present
            // --------------------------------------------------

            if (File.Exists(filePath))
            {
                Console.WriteLine(
                    "Removing existing copy of downloaded file..."
                );

                File.Delete(filePath);
            }

            // --------------------------------------------------
            // Step 6: Start download
            // --------------------------------------------------

            Console.WriteLine(
                $"Starting download: {filename}"
            );

            fileLink.Click();

            // --------------------------------------------------
            // Step 7: Wait for download to complete
            // --------------------------------------------------

            var downloadWait = new WebDriverWait(
                Driver,
                TimeSpan.FromSeconds(
                    DownloadTimeoutSeconds
                )
            );

            var downloadedFile = downloadWait.Until(driver =>
            {
                try
                {
                    if (!File.Exists(filePath))
                    {
                        return null;
                    }

                    var fileInfo = new FileInfo(filePath);

                    if (fileInfo.Length <= 0)
                    {
                        return null;
                    }

                    // Ensure Chrome has finished writing
                    // to the downloaded file.
                    using var stream = File.Open(
                        filePath,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Read
                    );

                    return fileInfo;
                }
                catch (IOException)
                {
                    // The file may still be in use by Chrome.
                    return null;
                }
            });

            // --------------------------------------------------
            // Step 8: Validate downloaded file
            // --------------------------------------------------

            Assert.That(
                downloadedFile,
                Is.Not.Null,
                $"Downloaded file was not found: {filename}"
            );

            Assert.That(
                downloadedFile!.Exists,
                Is.True,
                "Downloaded file should exist."
            );

            Assert.That(
                downloadedFile.Length,
                Is.GreaterThan(0),
                "Downloaded file should not be empty."
            );

            Console.WriteLine(
                "PASS - File downloaded successfully."
            );

            Console.WriteLine(
                $"   Filename: {downloadedFile.Name}"
            );

            Console.WriteLine(
                $"   File size: {downloadedFile.Length} bytes"
            );

            Console.WriteLine(
                $"   Location: {downloadedFile.FullName}"
            );
        }
    }
}
using System;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests.Tests
{
    [TestFixture]
    public class FileUploadTests : BaseTest
    {
        private const string TestFileName = "student.txt";

        private static readonly By UploadFieldLocator =
            By.Id("file-upload");

        private static readonly By SubmitButtonLocator =
            By.Id("file-submit");

        private static readonly By UploadedFileLocator =
            By.Id("uploaded-files");

        [Test]
        public void TestFileUpload()
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("TEST: File Upload");
            Console.WriteLine("==========================================");

            // --------------------------------------------------
            // Step 1: Prepare test file
            // --------------------------------------------------

            var testDataDirectory = Path.Combine(
                Directory.GetCurrentDirectory(),
                "TestData"
            );

            Directory.CreateDirectory(
                testDataDirectory
            );

            var filePath = Path.Combine(
                testDataDirectory,
                TestFileName
            );

            if (!File.Exists(filePath))
            {
                File.WriteAllText(
                    filePath,
                    "This is a Selenium file upload test."
                );

                Console.WriteLine(
                    $"Created test file: {filePath}"
                );
            }
            else
            {
                Console.WriteLine(
                    $"Using existing test file: {filePath}"
                );
            }

            var file = new FileInfo(filePath);

            Assert.That(
                file.Exists,
                Is.True,
                $"Test file should exist: {filePath}"
            );

            Assert.That(
                file.Length,
                Is.GreaterThan(0),
                "Test file should not be empty."
            );

            // --------------------------------------------------
            // Step 2: Navigate to upload page
            // --------------------------------------------------

            Driver.Navigate().GoToUrl(
                Config.Urls.TheInternet.FileUpload
            );

            Console.WriteLine(
                $"Navigated to: {Config.Urls.TheInternet.FileUpload}"
            );

            // --------------------------------------------------
            // Step 3: Create explicit wait
            // --------------------------------------------------

            var wait = new WebDriverWait(
                Driver,
                TimeSpan.FromSeconds(
                    Config.Selenium.DefaultTimeout
                )
            );

            // --------------------------------------------------
            // Step 4: Wait for upload field
            // --------------------------------------------------

            var uploadField = wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(
                        UploadFieldLocator
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
                uploadField,
                Is.Not.Null,
                "File upload field should be available."
            );

            // --------------------------------------------------
            // Step 5: Select file
            // --------------------------------------------------

            Console.WriteLine(
                $"Uploading file: {file.Name}"
            );

            uploadField!.SendKeys(
                file.FullName
            );

            Console.WriteLine(
                "File selected successfully."
            );

            // --------------------------------------------------
            // Step 6: Wait for submit button
            // --------------------------------------------------

            var submitButton = wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(
                        SubmitButtonLocator
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
                submitButton,
                Is.Not.Null,
                "File upload submit button should be available."
            );

            // --------------------------------------------------
            // Step 7: Submit upload
            // --------------------------------------------------

            submitButton!.Click();

            Console.WriteLine(
                "File upload submitted."
            );

            // --------------------------------------------------
            // Step 8: Wait for upload confirmation
            // --------------------------------------------------

            var uploadedFileElement = wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(
                        UploadedFileLocator
                    );

                    return element.Displayed
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
                uploadedFileElement,
                Is.Not.Null,
                "Upload confirmation should be displayed."
            );

            var uploadedFile =
                uploadedFileElement!.Text.Trim();

            Console.WriteLine(
                $"Uploaded file displayed as: {uploadedFile}"
            );

            // --------------------------------------------------
            // Step 9: Verify uploaded filename
            // --------------------------------------------------

            Assert.That(
                uploadedFile,
                Does.Contain(TestFileName),
                $"{TestFileName} was not uploaded successfully."
            );

            Console.WriteLine(
                $"PASS - {TestFileName} uploaded successfully."
            );

            Console.WriteLine(
                "PASS - File upload test completed successfully."
            );
        }
    }
}
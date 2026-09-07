using System;
using NUnit.Framework;
using SeleniumTests.Utilities;

namespace SeleniumTests.Tests
{
    [SetUpFixture]
    public class TestRunSetup
    {
        // ============================================================
        // ENTIRE TEST ASSEMBLY SETUP
        // ============================================================

        [OneTimeSetUp]
        public void StartTestRun()
        {
            Console.WriteLine();
            Console.WriteLine("==============================================");
            Console.WriteLine("        SELENIUM AUTOMATION TEST RUN");
            Console.WriteLine("==============================================");

            ReportHelper.Instance.StartTestRun();

            Console.WriteLine(
                "Test run reporting initialized."
            );
        }

        // ============================================================
        // ENTIRE TEST ASSEMBLY TEARDOWN
        // ============================================================

        [OneTimeTearDown]
        public void FinishTestRun()
        {
            try
            {
                Console.WriteLine();
                Console.WriteLine("==============================================");
                Console.WriteLine("        ALL TESTS HAVE FINISHED");
                Console.WriteLine("==============================================");

                Console.WriteLine(
                    "Generating final HTML report..."
                );

                var reportPath =
                    ReportHelper.Instance.GenerateReport();

                Console.WriteLine();
                Console.WriteLine(
                    "Opening final HTML report..."
                );

                ReportHelper.Instance.OpenReport(
                    reportPath
                );

                Console.WriteLine();
                Console.WriteLine(
                    "Final HTML report generated successfully."
                );

                Console.WriteLine(
                    $"Report location: {reportPath}"
                );

                Console.WriteLine(
                    "=============================================="
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error generating final test report: {ex.Message}"
                );
            }
        }
    }
}
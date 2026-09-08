using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using SeleniumTests.Config;

namespace SeleniumTests.Utilities
{
    public class TestResult
    {
        public string TestName { get; set; } = string.Empty;

        public bool Passed { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public string ScreenshotPath { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }

        public TimeSpan Duration { get; set; }
    }

    public class ReportHelper
    {
        // Keeps one shared report instance for the entire test run.
        private static readonly Lazy<ReportHelper> _instance =
            new(() => new ReportHelper());

        public static ReportHelper Instance =>
            _instance.Value;

        private readonly List<TestResult> _results = new();

        private readonly string _reportDirectory;

        private readonly string _screenshotDirectory;

        private DateTime _testStartTime;

        private ReportHelper()
        {
            var config = AppConfig.Instance;

            _reportDirectory = config.Reports.Directory;

            _screenshotDirectory = config.Screenshots.Directory;

            Directory.CreateDirectory(_reportDirectory);

            Directory.CreateDirectory(_screenshotDirectory);
        }

        // Starts a fresh reporting session before the tests begin.
        public void StartTestRun()
        {
            _testStartTime = DateTime.Now;

            _results.Clear();

            Console.WriteLine();
            Console.WriteLine("==============================================");
            Console.WriteLine("           TEST REPORTING STARTED");
            Console.WriteLine("==============================================");

            Console.WriteLine(
                $"Execution started: {_testStartTime:yyyy-MM-dd HH:mm:ss}"
            );

            Console.WriteLine();
        }

        // Stores the result of each test so it can be included in the final report.
        public void AddResult(TestResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(
                    nameof(result)
                );
            }

            _results.Add(result);

            Console.WriteLine(
                $"Report result recorded: {result.TestName} - " +
                $"{(result.Passed ? "PASSED" : "FAILED")}"
            );
        }

        // Builds the HTML report once the complete test run has finished.
        public string GenerateReport()
        {
            var endTime = DateTime.Now;

            var totalDuration =
                endTime - _testStartTime;

            var totalTests =
                _results.Count;

            var passedTests =
                _results.Count(
                    result => result.Passed
                );

            var failedTests =
                _results.Count(
                    result => !result.Passed
                );

            var passRate =
                totalTests > 0
                    ? (double)passedTests / totalTests * 100
                    : 0;

            var reportFilename =
                "TestReport.html";

            var reportPath =
                Path.Combine(
                    _reportDirectory,
                    reportFilename
                );

            var reportDirectoryFullPath =
                Path.GetFullPath(
                    _reportDirectory
                );

            var sb =
                new StringBuilder();

            // Build the report as a standalone HTML file so it can be opened directly.
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang='en'>");
            sb.AppendLine("<head>");

            sb.AppendLine(
                "<meta charset='UTF-8'>"
            );

            sb.AppendLine(
                "<meta name='viewport' content='width=device-width, initial-scale=1.0'>"
            );

            sb.AppendLine(
                "<title>Selenium Automation Test Report</title>"
            );

            sb.AppendLine("<style>");

            sb.AppendLine(@"
                * {
                    box-sizing: border-box;
                }

                html {
                    scroll-behavior: smooth;
                }

                body {
                    margin: 0;
                    padding: 0;
                    background: #f3f5f9;
                    color: #1f2937;
                    font-family:
                        -apple-system,
                        BlinkMacSystemFont,
                        'Segoe UI',
                        Roboto,
                        Helvetica,
                        Arial,
                        sans-serif;
                }

                .page {
                    min-height: 100vh;
                }

                .hero {
                    background:
                        linear-gradient(
                            135deg,
                            #111827 0%,
                            #1f2937 55%,
                            #374151 100%
                        );

                    color: white;
                    padding: 42px 30px;
                }

                .hero-content {
                    max-width: 1280px;
                    margin: 0 auto;
                }

                .brand {
                    display: flex;
                    align-items: center;
                    gap: 15px;
                    margin-bottom: 10px;
                }

                .brand-icon {
                    width: 48px;
                    height: 48px;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    border-radius: 12px;
                    background: rgba(255,255,255,0.12);
                    font-size: 25px;
                }

                .hero h1 {
                    margin: 0;
                    font-size: 30px;
                    font-weight: 700;
                    letter-spacing: -0.5px;
                }

                .hero-subtitle {
                    margin: 10px 0 0 63px;
                    color: #d1d5db;
                    font-size: 15px;
                }

                .content {
                    max-width: 1280px;
                    margin: -25px auto 50px;
                    padding: 0 25px;
                    position: relative;
                }

                .summary-grid {
                    display: grid;
                    grid-template-columns:
                        repeat(4, minmax(0, 1fr));

                    gap: 18px;
                    margin-bottom: 22px;
                }

                .summary-card {
                    background: white;
                    border-radius: 14px;
                    padding: 24px;
                    box-shadow:
                        0 4px 18px rgba(15,23,42,0.06);

                    border: 1px solid #e5e7eb;
                }

                .summary-card-header {
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    margin-bottom: 12px;
                }

                .summary-label {
                    color: #6b7280;
                    font-size: 13px;
                    font-weight: 600;
                    text-transform: uppercase;
                    letter-spacing: 0.5px;
                }

                .summary-icon {
                    font-size: 19px;
                }

                .summary-value {
                    font-size: 31px;
                    font-weight: 750;
                    line-height: 1;
                    color: #111827;
                }

                .summary-card.passed {
                    border-top: 4px solid #16a34a;
                }

                .summary-card.failed {
                    border-top: 4px solid #dc2626;
                }

                .summary-card.rate {
                    border-top: 4px solid #2563eb;
                }

                .execution-panel {
                    background: white;
                    border: 1px solid #e5e7eb;
                    border-radius: 14px;
                    padding: 22px 25px;
                    margin-bottom: 24px;

                    box-shadow:
                        0 4px 18px rgba(15,23,42,0.05);
                }

                .execution-header {
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    margin-bottom: 18px;
                }

                .section-title {
                    margin: 0;
                    font-size: 18px;
                    font-weight: 700;
                    color: #111827;
                }

                .execution-status {
                    padding: 6px 12px;
                    border-radius: 999px;
                    font-size: 12px;
                    font-weight: 700;
                }

                .execution-status.success {
                    background: #dcfce7;
                    color: #166534;
                }

                .execution-status.failure {
                    background: #fee2e2;
                    color: #991b1b;
                }

                .execution-grid {
                    display: grid;
                    grid-template-columns:
                        repeat(4, minmax(0, 1fr));

                    gap: 20px;
                }

                .execution-item-label {
                    display: block;
                    color: #6b7280;
                    font-size: 12px;
                    margin-bottom: 5px;
                }

                .execution-item-value {
                    color: #111827;
                    font-size: 14px;
                    font-weight: 600;
                }

                .progress-section {
                    margin-top: 20px;
                }

                .progress-header {
                    display: flex;
                    justify-content: space-between;
                    margin-bottom: 8px;
                    font-size: 13px;
                    font-weight: 600;
                }

                .progress-track {
                    height: 9px;
                    background: #e5e7eb;
                    border-radius: 999px;
                    overflow: hidden;
                }

                .progress-bar {
                    height: 100%;
                    background: #16a34a;
                    border-radius: 999px;
                }

                .results-panel {
                    background: white;
                    border: 1px solid #e5e7eb;
                    border-radius: 14px;

                    box-shadow:
                        0 4px 18px rgba(15,23,42,0.05);

                    overflow: hidden;
                }

                .results-header {
                    padding: 22px 25px;
                    border-bottom: 1px solid #e5e7eb;
                }

                .results-subtitle {
                    margin: 5px 0 0;
                    color: #6b7280;
                    font-size: 13px;
                }

                .results-table-wrapper {
                    overflow-x: auto;
                }

                table {
                    width: 100%;
                    border-collapse: collapse;
                }

                thead {
                    background: #f8fafc;
                }

                th {
                    padding: 13px 20px;
                    text-align: left;
                    color: #6b7280;
                    font-size: 11px;
                    text-transform: uppercase;
                    letter-spacing: 0.6px;
                    font-weight: 700;
                    border-bottom: 1px solid #e5e7eb;
                }

                td {
                    padding: 16px 20px;
                    border-bottom: 1px solid #f0f1f3;
                    font-size: 14px;
                    vertical-align: middle;
                }

                tbody tr:hover {
                    background: #f8fafc;
                }

                .test-name-cell {
                    color: #111827;
                    font-weight: 600;
                }

                .duration {
                    color: #4b5563;
                    font-variant-numeric: tabular-nums;
                }

                .timestamp {
                    color: #6b7280;
                    font-size: 13px;
                }

                .badge {
                    display: inline-flex;
                    align-items: center;
                    gap: 6px;
                    padding: 6px 10px;
                    border-radius: 999px;
                    font-size: 11px;
                    font-weight: 750;
                }

                .badge.pass {
                    background: #dcfce7;
                    color: #166534;
                }

                .badge.fail {
                    background: #fee2e2;
                    color: #991b1b;
                }

                details {
                    margin: 0;
                }

                summary {
                    cursor: pointer;
                    color: #2563eb;
                    font-size: 12px;
                    font-weight: 600;
                    user-select: none;
                }

                summary:hover {
                    text-decoration: underline;
                }

                .details-content {
                    padding: 20px;
                    background: #f8fafc;
                    border-top: 1px solid #e5e7eb;
                }

                .error-box {
                    padding: 14px;
                    border-radius: 9px;
                    background: #fff1f2;
                    border: 1px solid #fecdd3;
                    color: #9f1239;
                    white-space: pre-wrap;
                    font-family:
                        Consolas,
                        'Courier New',
                        monospace;
                    font-size: 12px;
                    line-height: 1.6;
                    overflow-x: auto;
                }

                .screenshot-container {
                    margin-top: 18px;
                }

                .screenshot-label {
                    font-size: 12px;
                    font-weight: 700;
                    color: #374151;
                    margin-bottom: 9px;
                }

                .screenshot-container img {
                    display: block;
                    max-width: 100%;
                    max-height: 650px;
                    border-radius: 10px;
                    border: 1px solid #d1d5db;
                    box-shadow:
                        0 3px 12px rgba(15,23,42,0.08);
                    cursor: zoom-in;
                }

                .empty-state {
                    padding: 50px;
                    text-align: center;
                    color: #6b7280;
                }

                .empty-icon {
                    font-size: 38px;
                    margin-bottom: 12px;
                }

                .footer {
                    max-width: 1280px;
                    margin: 25px auto 45px;
                    padding: 0 25px;
                    text-align: center;
                    color: #9ca3af;
                    font-size: 12px;
                }

                @media (max-width: 1000px) {
                    .summary-grid {
                        grid-template-columns:
                            repeat(2, minmax(0, 1fr));
                    }

                    .execution-grid {
                        grid-template-columns:
                            repeat(2, minmax(0, 1fr));
                    }
                }

                @media (max-width: 650px) {
                    .hero {
                        padding: 30px 20px;
                    }

                    .hero h1 {
                        font-size: 23px;
                    }

                    .hero-subtitle {
                        margin-left: 0;
                    }

                    .content {
                        padding: 0 15px;
                    }

                    .summary-grid {
                        grid-template-columns: 1fr;
                    }

                    .execution-grid {
                        grid-template-columns: 1fr;
                    }

                    .brand-icon {
                        display: none;
                    }

                    th,
                    td {
                        padding: 12px;
                    }
                }
            ");

            sb.AppendLine("</style>");
            sb.AppendLine("</head>");

            sb.AppendLine("<body>");
            sb.AppendLine("<div class='page'>");

            sb.AppendLine("<header class='hero'>");
            sb.AppendLine("<div class='hero-content'>");

            sb.AppendLine("<div class='brand'>");

            sb.AppendLine(
                "<div class='brand-icon'>⚙️</div>"
            );

            sb.AppendLine(
                "<h1>Selenium Automation Test Report</h1>"
            );

            sb.AppendLine("</div>");

            sb.AppendLine(
                "<div class='hero-subtitle'>" +
                "Automated UI test execution summary" +
                "</div>"
            );

            sb.AppendLine("</div>");
            sb.AppendLine("</header>");

            sb.AppendLine("<main class='content'>");

            sb.AppendLine("<section class='summary-grid'>");

            sb.AppendLine(
                $"<div class='summary-card'>" +
                "<div class='summary-card-header'>" +
                "<span class='summary-label'>Total Tests</span>" +
                "<span class='summary-icon'>🧪</span>" +
                "</div>" +
                $"<div class='summary-value'>{totalTests}</div>" +
                "</div>"
            );

            sb.AppendLine(
                $"<div class='summary-card passed'>" +
                "<div class='summary-card-header'>" +
                "<span class='summary-label'>Passed</span>" +
                "<span class='summary-icon'>✓</span>" +
                "</div>" +
                $"<div class='summary-value'>{passedTests}</div>" +
                "</div>"
            );

            sb.AppendLine(
                $"<div class='summary-card failed'>" +
                "<div class='summary-card-header'>" +
                "<span class='summary-label'>Failed</span>" +
                "<span class='summary-icon'>!</span>" +
                "</div>" +
                $"<div class='summary-value'>{failedTests}</div>" +
                "</div>"
            );

            sb.AppendLine(
                $"<div class='summary-card rate'>" +
                "<div class='summary-card-header'>" +
                "<span class='summary-label'>Pass Rate</span>" +
                "<span class='summary-icon'>%</span>" +
                "</div>" +
                $"<div class='summary-value'>{passRate:F1}%</div>" +
                "</div>"
            );

            sb.AppendLine("</section>");

            // Shows whether the overall automation run completed successfully.
            var executionSuccessful =
                failedTests == 0 &&
                totalTests > 0;

            var executionStatusClass =
                executionSuccessful
                    ? "success"
                    : "failure";

            var executionStatusText =
                executionSuccessful
                    ? "ALL TESTS PASSED"
                    : "TESTS FAILED";

            sb.AppendLine(
                "<section class='execution-panel'>"
            );

            sb.AppendLine(
                "<div class='execution-header'>"
            );

            sb.AppendLine(
                "<h2 class='section-title'>" +
                "Execution Summary" +
                "</h2>"
            );

            sb.AppendLine(
                $"<span class='execution-status {executionStatusClass}'>" +
                $"{executionStatusText}" +
                "</span>"
            );

            sb.AppendLine("</div>");

            sb.AppendLine(
                "<div class='execution-grid'>"
            );

            sb.AppendLine(
                "<div>" +
                "<span class='execution-item-label'>Started</span>" +
                $"<span class='execution-item-value'>" +
                $"{_testStartTime:yyyy-MM-dd HH:mm:ss}" +
                "</span>" +
                "</div>"
            );

            sb.AppendLine(
                "<div>" +
                "<span class='execution-item-label'>Completed</span>" +
                $"<span class='execution-item-value'>" +
                $"{endTime:yyyy-MM-dd HH:mm:ss}" +
                "</span>" +
                "</div>"
            );

            sb.AppendLine(
                "<div>" +
                "<span class='execution-item-label'>Total Duration</span>" +
                $"<span class='execution-item-value'>" +
                $"{totalDuration.TotalSeconds:F2} seconds" +
                "</span>" +
                "</div>"
            );

            sb.AppendLine(
                "<div>" +
                "<span class='execution-item-label'>Average Test Duration</span>" +
                $"<span class='execution-item-value'>" +
                $"{GetAverageDuration():F2} seconds" +
                "</span>" +
                "</div>"
            );

            sb.AppendLine("</div>");

            // Gives a quick visual indication of the overall pass rate.
            sb.AppendLine(
                "<div class='progress-section'>"
            );

            sb.AppendLine(
                "<div class='progress-header'>"
            );

            sb.AppendLine(
                "<span>Overall Pass Rate</span>"
            );

            sb.AppendLine(
                $"<span>{passRate:F1}%</span>"
            );

            sb.AppendLine("</div>");

            sb.AppendLine(
                "<div class='progress-track'>"
            );

            sb.AppendLine(
                $"<div class='progress-bar' " +
                $"style='width:{passRate:F1}%'></div>"
            );

            sb.AppendLine("</div>");
            sb.AppendLine("</div>");

            sb.AppendLine("</section>");

            sb.AppendLine(
                "<section class='results-panel'>"
            );

            sb.AppendLine(
                "<div class='results-header'>"
            );

            sb.AppendLine(
                "<h2 class='section-title'>" +
                "Test Results" +
                "</h2>"
            );

            sb.AppendLine(
                "<p class='results-subtitle'>" +
                "Detailed results for the complete automation run" +
                "</p>"
            );

            sb.AppendLine("</div>");

            if (_results.Count == 0)
            {
                sb.AppendLine(
                    "<div class='empty-state'>"
                );

                sb.AppendLine(
                    "<div class='empty-icon'>📭</div>"
                );

                sb.AppendLine(
                    "<strong>No test results recorded</strong>"
                );

                sb.AppendLine(
                    "<p>No tests were recorded during this execution.</p>"
                );

                sb.AppendLine("</div>");
            }
            else
            {
                sb.AppendLine(
                    "<div class='results-table-wrapper'>"
                );

                sb.AppendLine("<table>");

                sb.AppendLine("<thead>");
                sb.AppendLine("<tr>");

                sb.AppendLine("<th>Test</th>");
                sb.AppendLine("<th>Status</th>");
                sb.AppendLine("<th>Duration</th>");
                sb.AppendLine("<th>Executed</th>");
                sb.AppendLine("<th>Details</th>");

                sb.AppendLine("</tr>");
                sb.AppendLine("</thead>");

                sb.AppendLine("<tbody>");

                foreach (
                    var result in
                    _results.OrderBy(
                        result => result.Timestamp
                    )
                )
                {
                    var statusClass =
                        result.Passed
                            ? "pass"
                            : "fail";

                    var statusText =
                        result.Passed
                            ? "✓ PASSED"
                            : "✕ FAILED";

                    sb.AppendLine("<tr>");

                    sb.AppendLine(
                        $"<td class='test-name-cell'>" +
                        $"{HtmlEncode(result.TestName)}" +
                        "</td>"
                    );

                    sb.AppendLine(
                        $"<td>" +
                        $"<span class='badge {statusClass}'>" +
                        $"{statusText}" +
                        "</span>" +
                        "</td>"
                    );

                    sb.AppendLine(
                        $"<td class='duration'>" +
                        $"{result.Duration.TotalSeconds:F2}s" +
                        "</td>"
                    );

                    sb.AppendLine(
                        $"<td class='timestamp'>" +
                        $"{result.Timestamp:HH:mm:ss}" +
                        "</td>"
                    );

                    sb.AppendLine("<td>");

                    sb.AppendLine("<details>");

                    sb.AppendLine(
                        "<summary>View details</summary>"
                    );

                    sb.AppendLine(
                        "<div class='details-content'>"
                    );

                    if (
                        !result.Passed &&
                        !string.IsNullOrWhiteSpace(
                            result.ErrorMessage
                        )
                    )
                    {
                        sb.AppendLine(
                            "<div class='error-box'>"
                        );

                        sb.AppendLine(
                            $"<strong>Error</strong><br><br>" +
                            $"{HtmlEncode(result.ErrorMessage)}"
                        );

                        sb.AppendLine(
                            "</div>"
                        );
                    }

                    if (
                        !string.IsNullOrWhiteSpace(
                            result.ScreenshotPath
                        )
                    )
                    {
                        var screenshotLink =
                            GetRelativeScreenshotPath(
                                reportDirectoryFullPath,
                                result.ScreenshotPath
                            );

                        if (
                            !string.IsNullOrWhiteSpace(
                                screenshotLink
                            )
                        )
                        {
                            sb.AppendLine(
                                "<div class='screenshot-container'>"
                            );

                            sb.AppendLine(
                                "<div class='screenshot-label'>" +
                                "Test Screenshot" +
                                "</div>"
                            );

                            sb.AppendLine(
                                $"<a href='{HtmlEncode(screenshotLink)}' " +
                                "target='_blank'>"
                            );

                            sb.AppendLine(
                                $"<img src='{HtmlEncode(screenshotLink)}' " +
                                $"alt='Screenshot for {HtmlEncode(result.TestName)}'>"
                            );

                            sb.AppendLine(
                                "</a>"
                            );

                            sb.AppendLine(
                                "</div>"
                            );
                        }
                    }

                    sb.AppendLine(
                        "</div>"
                    );

                    sb.AppendLine("</details>");

                    sb.AppendLine("</td>");

                    sb.AppendLine("</tr>");
                }

                sb.AppendLine("</tbody>");
                sb.AppendLine("</table>");

                sb.AppendLine("</div>");
            }

            sb.AppendLine("</section>");

            sb.AppendLine("</main>");

            sb.AppendLine(
                "<footer class='footer'>"
            );

            sb.AppendLine(
                "<div>" +
                "SeleniumTests Automation Framework" +
                "</div>"
            );

            sb.AppendLine(
                $"<div>Report generated {endTime:yyyy-MM-dd HH:mm:ss}</div>"
            );

            sb.AppendLine(
                "</footer>"
            );

            sb.AppendLine("</div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            // Save the completed report to the configured reports folder.
            File.WriteAllText(
                reportPath,
                sb.ToString(),
                Encoding.UTF8
            );

            var fullReportPath =
                Path.GetFullPath(
                    reportPath
                );

            Console.WriteLine();
            Console.WriteLine("==============================================");
            Console.WriteLine("           FINAL REPORT GENERATED");
            Console.WriteLine("==============================================");

            Console.WriteLine(
                $"Report: {fullReportPath}"
            );

            Console.WriteLine(
                $"Total tests: {totalTests}"
            );

            Console.WriteLine(
                $"Passed: {passedTests}"
            );

            Console.WriteLine(
                $"Failed: {failedTests}"
            );

            Console.WriteLine(
                $"Pass rate: {passRate:F1}%"
            );

            Console.WriteLine(
                $"Execution time: {totalDuration.TotalSeconds:F2}s"
            );

            Console.WriteLine(
                "=============================================="
            );

            return fullReportPath;
        }

        // Calculates the average duration across all recorded tests.
        private double GetAverageDuration()
        {
            if (_results.Count == 0)
            {
                return 0;
            }

            return _results.Average(
                result => result.Duration.TotalSeconds
            );
        }

        // Opens the generated report using the user's default browser.
        public void OpenReport(string reportPath)
        {
            if (string.IsNullOrWhiteSpace(reportPath))
            {
                Console.WriteLine(
                    "Report path is empty. Cannot open report."
                );

                return;
            }

            try
            {
                var fullPath =
                    Path.GetFullPath(
                        reportPath
                    );

                if (!File.Exists(fullPath))
                {
                    Console.WriteLine(
                        $"Report file does not exist: {fullPath}"
                    );

                    return;
                }

                Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = fullPath,
                        UseShellExecute = true
                    }
                );

                Console.WriteLine(
                    $"Final report opened: {fullPath}"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Unable to open report automatically: {ex.Message}"
                );
            }
        }

        // Converts the screenshot path into a path that works from the HTML report.
        private static string GetRelativeScreenshotPath(
            string reportDirectory,
            string screenshotPath)
        {
            try
            {
                var fullScreenshotPath =
                    Path.GetFullPath(
                        screenshotPath
                    );

                if (!File.Exists(fullScreenshotPath))
                {
                    return string.Empty;
                }

                var relativePath =
                    Path.GetRelativePath(
                        reportDirectory,
                        fullScreenshotPath
                    );

                return relativePath.Replace(
                    "\\",
                    "/"
                );
            }
            catch
            {
                return string.Empty;
            }
        }

        // Prevents test names and error messages from breaking the HTML markup.
        private static string HtmlEncode(
            string value)
        {
            return WebUtility.HtmlEncode(
                value ?? string.Empty
            );
        }
    }
}
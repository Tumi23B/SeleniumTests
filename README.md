# SeleniumTests

A maintainable UI test automation framework built with **C#, .NET, Selenium WebDriver, and NUnit**.

This project demonstrates practical web UI automation using the **Page Object Model (POM)**, reusable Selenium utilities, externalized configuration, automated screenshots, and a custom HTML test execution report.

---

## Overview

SeleniumTests is a browser automation framework designed to provide a structured and maintainable approach to automated UI testing.

The framework separates test scenarios from browser interaction by using Page Objects and reusable utilities.

It also provides automated test evidence through screenshots and a consolidated HTML execution report.

### Key Features

- Page Object Model architecture
- Reusable Selenium WebDriver utilities
- NUnit test framework
- Explicit waits
- Externalized JSON configuration
- Local credential configuration
- Chrome browser automation
- Selenium Manager for WebDriver management
- Automated screenshots
- Centralized HTML test reporting
- Test execution duration tracking
- Pass/fail reporting
- Error message capture
- Screenshot evidence for test executions
- Automatic report generation after the complete test run

---

## Tech Stack

| Technology | Purpose |
|---|---|
| C# | Programming language |
| .NET 10 | Application and test framework |
| Selenium WebDriver | Browser automation |
| NUnit | Test framework |
| NUnit3TestAdapter | NUnit integration with .NET test tooling |
| Microsoft.NET.Test.Sdk | Test execution |
| Selenium Manager | WebDriver management |
| Chrome | Automated browser |
| JSON | Application and test configuration |

---

## Project Structure

```text
SeleniumTests/
│
├── Config/
│   └── AppConfig.cs
│
├── Pages/
│   ├── BasePage.cs
│   ├── LoginPage.cs
│   └── SauceDemoPage.cs
│
├── Tests/
│   ├── BaseTest.cs
│   ├── TestRunSetup.cs
│   ├── LoginTests.cs
│   ├── SauceDemoTests.cs
│   ├── CheckboxTests.cs
│   ├── DropdownTests.cs
│   ├── FileUploadTests.cs
│   └── FileDownloadTests.cs
│
├── Utilities/
│   ├── WebDriverManager.cs
│   ├── ScreenshotHelper.cs
│   └── ReportHelper.cs
│
├── TestData/
│
├── Downloads/
│
├── Screenshots/
│
├── Reports/
│
├── appsettings.json
├── appsettings.example.json
├── appsettings.local.json
├── .gitignore
└── SeleniumTests.csproj

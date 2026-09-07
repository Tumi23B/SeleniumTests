# SeleniumTests

A C# UI test automation project built with **.NET, Selenium WebDriver, and NUnit**.

This project demonstrates practical web UI automation using the **Page Object Model (POM)**, reusable Selenium utilities, externalized configuration, automated screenshots, and custom HTML test reporting.

---

## Overview

SeleniumTests is a browser-based test automation project designed to demonstrate a structured and maintainable approach to automated UI testing.

The project separates test scenarios from browser interaction through Page Objects and reusable utilities.

It also provides automated test evidence through screenshots and a consolidated HTML execution report.

### Key Features

* Page Object Model architecture
* Reusable Selenium WebDriver utilities
* NUnit test automation
* Explicit waits
* Externalized JSON configuration
* Local credential configuration
* Chrome browser automation
* Selenium Manager for WebDriver management
* Automated screenshots
* Centralized HTML test reporting
* Test execution duration tracking
* Pass/fail reporting
* Error message capture
* Screenshot evidence for test executions
* Automatic report generation after the complete test run

---

## Tech Stack

| Technology              | Purpose                            |
| ----------------------- | ---------------------------------- |
| C#                      | Programming language               |
| .NET 10                 | Application and test runtime       |
| Selenium WebDriver 4.48 | Browser automation                 |
| NUnit 4.3               | Test framework                     |
| NUnit3TestAdapter       | NUnit integration                  |
| Microsoft.NET.Test.Sdk  | Test execution                     |
| Selenium Manager        | WebDriver management               |
| Google Chrome           | Automated browser                  |
| JSON                    | Application and test configuration |

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
```

---

## Project Architecture

The project uses a simple separation of responsibilities between tests, Page Objects, and reusable utilities.

```text
                    SeleniumTests
                         │
             ┌───────────┴───────────┐
             │                       │
           Tests                   Pages
             │                       │
             │                 Page Objects
             │                       │
             └───────────┬───────────┘
                         │
                  Selenium WebDriver
                         │
                       Chrome
                         │
                   Web Application
```

Supporting utilities handle browser creation, screenshots, configuration, and reporting.

---

## Page Object Model

The project uses the **Page Object Model** to keep browser interaction separate from test scenarios.

Page Objects are responsible for:

* Locators
* Page navigation
* User interactions
* Page-specific operations
* Application-level validation

Test classes are responsible for:

* Test scenarios
* Test execution flow
* Assertions
* Expected behaviour

This reduces duplicated Selenium code and makes the tests easier to maintain when application elements change.

---

## Base Page

`BasePage` provides common Selenium functionality shared by the Page Objects.

It includes reusable functionality for:

* Waiting for elements
* Waiting for elements to become visible
* Waiting for elements to become clickable
* Clicking elements
* Entering text
* Retrieving element text
* Checking element visibility
* Scrolling
* Waiting for URL changes
* Waiting for title changes
* Navigation
* Configurable visual pauses

This keeps common Selenium operations in one place instead of duplicating them throughout individual Page Objects.

---

## WebDriver Management

`WebDriverManager` is responsible for creating and configuring Chrome WebDriver instances.

The browser configuration includes options for:

* Maximized browser execution
* Disabled notifications
* Disabled extensions
* Disabled pop-ups
* Automated file downloads
* Headless execution when required

The project uses **Selenium Manager** to manage the required browser driver, eliminating the need to maintain a separate ChromeDriver executable in the repository.

---

## Configuration

Application and test configuration is maintained outside of the test classes.

Configuration includes:

* Application URLs
* Test credentials
* Selenium timeout settings
* Visual pause duration
* Headless browser settings
* Screenshot directory
* Report directory
* Application messages

The main configuration file is:

```text
appsettings.json
```

A safe configuration template is provided through:

```text
appsettings.example.json
```

Local credentials are stored separately in:

```text
appsettings.local.json
```

The local configuration file is excluded from source control.

### Configuration Structure

```text
appsettings.json
        │
        ├── URLs
        ├── Selenium settings
        ├── Screenshot settings
        ├── Report settings
        └── Application messages

appsettings.local.json
        │
        └── Local test credentials
```

---

## Test Coverage

The project contains automated UI test scenarios covering authentication, e-commerce workflows, browser controls, and file operations.

### Login Tests

The login test suite covers:

* Login with valid credentials
* Login and logout with valid credentials
* Login with incorrect credentials
* Login with invalid username
* Login with invalid password
* Login with empty credentials
* Login success message validation
* Logout success message validation
* Login error message validation

These scenarios demonstrate both successful and unsuccessful authentication flows.

---

### SauceDemo Tests

The SauceDemo test scenarios cover common e-commerce workflows, including:

* User authentication
* Product selection
* Adding products to the cart
* Removing products from the cart
* Cart validation
* Checkout
* Customer information entry
* Order completion

The application interactions are handled through the `SauceDemoPage` Page Object.

---

### Checkbox Tests

The checkbox tests demonstrate:

* Identifying checkbox elements
* Checking checkbox states
* Selecting checkboxes
* Deselecting checkboxes
* Validating expected states

---

### Dropdown Tests

The dropdown tests demonstrate:

* Locating dropdown controls
* Selecting options
* Validating selected options
* Working with Selenium's `SelectElement`

---

### File Upload Tests

The file upload tests demonstrate:

* Locating file upload controls
* Providing file paths
* Uploading files
* Validating upload behaviour

---

### File Download Tests

The file download tests demonstrate:

* Triggering browser downloads
* Configuring the Chrome download directory
* Monitoring downloaded files
* Validating downloaded content

---

## Explicit Waits

The project uses explicit waits for normal Selenium synchronization instead of relying on fixed delays.

Reusable wait functionality is provided through `BasePage`.

The project supports waiting for:

* Elements to exist
* Elements to become visible
* Elements to become clickable
* URLs to contain expected values
* Page titles to contain expected values

This helps reduce timing-related test failures and improves automation stability.

---

## Automated Screenshots

A screenshot is automatically captured for each test execution.

Screenshots are stored in:

```text
Screenshots/
```

Screenshot filenames contain the test name and execution timestamp.

Example:

```text
LoginAndLogoutWithValidCredentials_2026-09-07_18-30-15-123.png
```

Screenshots provide visual evidence of the browser state at the end of each test.

---

## HTML Test Reporting

After the complete test suite finishes, the project generates **one consolidated HTML test report**.

The report is generated at:

```text
Reports/TestReport.html
```

The report includes:

* Total number of tests
* Passed tests
* Failed tests
* Pass rate
* Test execution start time
* Test execution completion time
* Total execution duration
* Average test duration
* Individual test duration
* Test execution timestamp
* Test status
* Error messages
* Screenshot evidence

The report is automatically opened in the browser after the complete test run.

---

## Test Reporting Flow

```text
Test Execution Starts
        │
        ▼
Run Test
        │
        ├── Pass / Fail
        ├── Capture Duration
        └── Capture Screenshot
        │
        ▼
Continue Through Test Suite
        │
        ▼
All Tests Complete
        │
        ▼
Generate TestReport.html
        │
        ▼
Open Report Automatically
```

This provides a single view of the complete test execution rather than generating separate reports for individual test classes.

---

## Error Handling

When a test fails, the test result captures the available error information.

The HTML report displays the failure details together with the associated screenshot evidence.

This makes it easier to identify:

* Which test failed
* When the test executed
* How long it ran
* What error occurred
* What the browser looked like during the test

---

## Test Lifecycle

The tests follow a common lifecycle for browser setup, execution, evidence collection, and cleanup.

```text
Test Run
   │
   ▼
Start Reporting
   │
   ▼
Create WebDriver
   │
   ▼
Run Test
   │
   ▼
Capture Test Result
   │
   ▼
Capture Screenshot
   │
   ▼
Dispose WebDriver
   │
   ▼
Continue With Next Test
   │
   ▼
All Tests Complete
   │
   ▼
Generate HTML Report
   │
   ▼
Open Report
```

---

## Running the Project

Make sure the .NET SDK and Google Chrome are installed.

Open a terminal in the project directory.

### Restore Dependencies

```bash
dotnet restore
```

### Build the Project

```bash
dotnet build
```

### Run All Tests

```bash
dotnet test
```

The complete test suite will execute and the HTML report will be generated after the run finishes.

---

## Running Specific Tests

To run the login tests:

```bash
dotnet test --filter "FullyQualifiedName~LoginTests"
```

To run the SauceDemo tests:

```bash
dotnet test --filter "FullyQualifiedName~SauceDemoTests"
```

Specific tests can also be selected using NUnit test filtering supported by the .NET test runner.

---

## Browser Configuration

Chrome is used as the default browser.

By default, the browser runs visibly so that the test execution can be observed.

Headless execution can be enabled through configuration when required.

Example:

```json
"Selenium": {
  "DefaultTimeout": 10,
  "VisualPause": 1000,
  "Headless": true
}
```

---

## Test Artifacts

The project generates several local test artifacts.

```text
Screenshots/
    └── Test screenshots

Reports/
    └── TestReport.html

Downloads/
    └── Downloaded test files
```

These files are generated during test execution and are excluded from source control.

---

## Security

Test credentials are not stored directly inside the test classes.

Local credentials should be stored in:

```text
appsettings.local.json
```

This file is excluded through `.gitignore`.

The repository contains only the safe configuration template:

```text
appsettings.example.json
```

**Never commit real credentials to source control.**

---

## Source Control

The following local and generated files are excluded from the Git repository:

```text
appsettings.local.json
bin/
obj/
.vs/
.vscode/
Screenshots/
Reports/
Downloads/
```

This keeps the repository focused on the source code and prevents local credentials and generated test artifacts from being committed.

---

## Test Execution Example

A typical automated test follows this flow:

```text
Create Page Object
        │
        ▼
Navigate to Application
        │
        ▼
Perform User Actions
        │
        ▼
Validate Expected Behaviour
        │
        ▼
Capture Test Result
        │
        ├── Pass / Fail
        ├── Duration
        └── Screenshot
```

---

## Goals

The goal of this project is to demonstrate practical UI automation and software testing skills using modern C# and Selenium technologies.

The project focuses on:

* Maintainable Selenium automation
* Page Object Model
* Reusable automation utilities
* Explicit waits
* Externalized configuration
* Secure handling of local credentials
* Automated screenshot evidence
* HTML test reporting
* NUnit test organisation
* Clear separation between test scenarios and page interaction

---

## Future Improvements

Potential improvements for the project include:

* Cross-browser testing
* Parallel test execution
* CI/CD pipeline integration
* API test integration
* Database validation
* Advanced logging
* Allure or Extent reporting
* Docker-based test execution
* Environment-specific configuration
* Additional test data management
* Retry handling for transient failures

---

## Author

**Boitumelo Khauoe**

Junior Software Developer | Software Testing & Automation

South Africa

GitHub: **Tumi23B**

---

## Project Status

**Active Development**

The project is being continuously improved as additional automated scenarios, utilities, and testing capabilities are added.

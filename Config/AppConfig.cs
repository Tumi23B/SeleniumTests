using Newtonsoft.Json;
using System;
using System.IO;

namespace SeleniumTests.Config
{
    public class AppConfig
    {
        public CredentialConfig Credentials { get; set; } = new();
        public UrlConfig Urls { get; set; } = new();
        public SeleniumConfig Selenium { get; set; } = new();
        public ScreenshotConfig Screenshots { get; set; } = new();
        public ReportConfig Reports { get; set; } = new();
        public MessageConfig Messages { get; set; } = new();

        private static AppConfig? _instance;
        private static readonly object _lock = new();

        public static AppConfig Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = LoadConfiguration();
                    }

                    return _instance;
                }
            }
        }

        private static AppConfig LoadConfiguration()
        {
            var basePath = AppContext.BaseDirectory;

            var baseConfigPath =
                Path.Combine(basePath, "appsettings.json");

            var localConfigPath =
                Path.Combine(basePath, "appsettings.local.json");

            if (!File.Exists(baseConfigPath))
            {
                throw new FileNotFoundException(
                    "appsettings.json was not found.",
                    baseConfigPath
                );
            }

            // Load the safe/public configuration
            var baseJson =
                File.ReadAllText(baseConfigPath);

            var config =
                JsonConvert.DeserializeObject<AppConfig>(
                    baseJson
                ) ?? new AppConfig();

            // Load local configuration if it exists
            if (File.Exists(localConfigPath))
            {
                var localJson =
                    File.ReadAllText(localConfigPath);

                var localConfig =
                    JsonConvert.DeserializeObject<AppConfig>(
                        localJson
                    );

                if (localConfig != null)
                {
                    MergeLocalConfiguration(
                        config,
                        localConfig
                    );
                }
            }

            return config;
        }

        private static void MergeLocalConfiguration(
            AppConfig config,
            AppConfig localConfig)
        {
            // --------------------------------------------------
            // Credentials
            // --------------------------------------------------

            if (localConfig.Credentials?.Login != null)
            {
                config.Credentials ??= new CredentialConfig();
                config.Credentials.Login ??= new LoginCredentials();

                if (!string.IsNullOrWhiteSpace(
                    localConfig.Credentials.Login.ValidUsername))
                {
                    config.Credentials.Login.ValidUsername =
                        localConfig.Credentials.Login.ValidUsername;
                }

                if (!string.IsNullOrWhiteSpace(
                    localConfig.Credentials.Login.ValidPassword))
                {
                    config.Credentials.Login.ValidPassword =
                        localConfig.Credentials.Login.ValidPassword;
                }

                if (!string.IsNullOrWhiteSpace(
                    localConfig.Credentials.Login.InvalidUsername))
                {
                    config.Credentials.Login.InvalidUsername =
                        localConfig.Credentials.Login.InvalidUsername;
                }

                if (!string.IsNullOrWhiteSpace(
                    localConfig.Credentials.Login.InvalidPassword))
                {
                    config.Credentials.Login.InvalidPassword =
                        localConfig.Credentials.Login.InvalidPassword;
                }
            }

            // --------------------------------------------------
            // URLs
            // --------------------------------------------------

            if (localConfig.Urls?.TheInternet != null)
            {
                config.Urls ??= new UrlConfig();
                config.Urls.TheInternet ??= new TheInternetUrls();

                if (!string.IsNullOrWhiteSpace(
                    localConfig.Urls.TheInternet.Login))
                {
                    config.Urls.TheInternet.Login =
                        localConfig.Urls.TheInternet.Login;
                }

                if (!string.IsNullOrWhiteSpace(
                    localConfig.Urls.TheInternet.Checkboxes))
                {
                    config.Urls.TheInternet.Checkboxes =
                        localConfig.Urls.TheInternet.Checkboxes;
                }

                if (!string.IsNullOrWhiteSpace(
                    localConfig.Urls.TheInternet.Dropdown))
                {
                    config.Urls.TheInternet.Dropdown =
                        localConfig.Urls.TheInternet.Dropdown;
                }

                if (!string.IsNullOrWhiteSpace(
                    localConfig.Urls.TheInternet.FileUpload))
                {
                    config.Urls.TheInternet.FileUpload =
                        localConfig.Urls.TheInternet.FileUpload;
                }

                if (!string.IsNullOrWhiteSpace(
                    localConfig.Urls.TheInternet.FileDownload))
                {
                    config.Urls.TheInternet.FileDownload =
                        localConfig.Urls.TheInternet.FileDownload;
                }
            }

            if (localConfig.Urls?.SauceDemo != null)
            {
                config.Urls ??= new UrlConfig();
                config.Urls.SauceDemo ??= new SauceDemoUrls();

                if (!string.IsNullOrWhiteSpace(
                    localConfig.Urls.SauceDemo.Base))
                {
                    config.Urls.SauceDemo.Base =
                        localConfig.Urls.SauceDemo.Base;
                }
            }
        }
    }

    // ==========================================================
    // CREDENTIALS
    // ==========================================================

    public class CredentialConfig
    {
        public LoginCredentials Login { get; set; } = new();
    }

    public class LoginCredentials
    {
        public string ValidUsername { get; set; } = string.Empty;
        public string ValidPassword { get; set; } = string.Empty;
        public string InvalidUsername { get; set; } = string.Empty;
        public string InvalidPassword { get; set; } = string.Empty;
    }

    // ==========================================================
    // URLS
    // ==========================================================

    public class UrlConfig
    {
        public TheInternetUrls TheInternet { get; set; } = new();
        public SauceDemoUrls SauceDemo { get; set; } = new();
    }

    public class TheInternetUrls
    {
        public string Login { get; set; } = string.Empty;
        public string Checkboxes { get; set; } = string.Empty;
        public string Dropdown { get; set; } = string.Empty;
        public string FileUpload { get; set; } = string.Empty;
        public string FileDownload { get; set; } = string.Empty;
    }

    public class SauceDemoUrls
    {
        public string Base { get; set; } = string.Empty;
    }

    // ==========================================================
    // SELENIUM
    // ==========================================================

    public class SeleniumConfig
    {
        public int DefaultTimeout { get; set; } = 10;
        public int VisualPause { get; set; } = 1000;
        public bool Headless { get; set; } = false;
    }

    // ==========================================================
    // SCREENSHOTS
    // ==========================================================

    public class ScreenshotConfig
    {
        public string Directory { get; set; } = "Screenshots";
    }

    // ==========================================================
    // REPORTS
    // ==========================================================

    public class ReportConfig
    {
        public string Directory { get; set; } = "Reports";
    }

    // ==========================================================
    // MESSAGES
    // ==========================================================

    public class MessageConfig
    {
        public string LoginSuccess { get; set; } = string.Empty;
        public string LogoutSuccess { get; set; } = string.Empty;
        public string LoginError { get; set; } = string.Empty;
        public string PasswordError { get; set; } = string.Empty;
    }
}
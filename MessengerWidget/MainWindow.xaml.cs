using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Windows;

namespace MessengerWidget
{
    public partial class MainWindow : Window
    {
        // App.xaml.cs Exit-hez
        public bool AllowClose { get; set; } = false;

        private const string PrimaryUrl = "https://www.messenger.com/";
        private const string FallbackUrl = "https://www.facebook.com/messages";

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var userDataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MessengerWidget",
                "WebView2");

            var environment = await CoreWebView2Environment.CreateAsync(null, userDataFolder);
            await WebView.EnsureCoreWebView2Async(environment);

            SetupWebView();

            // Jelenleg ez az “élő”:
            WebView.CoreWebView2.Navigate(FallbackUrl);

            // Ha majd vissza akarsz váltani messenger.com-ra:
            // WebView.CoreWebView2.Navigate(PrimaryUrl);
        }

        private void SetupWebView()
        {
            var core = WebView.CoreWebView2;

            core.PermissionRequested += (_, e) =>
            {
                if (e.PermissionKind == CoreWebView2PermissionKind.Camera ||
                    e.PermissionKind == CoreWebView2PermissionKind.Microphone)
                {
                    e.State = CoreWebView2PermissionState.Allow;
                }
            };

            core.NewWindowRequested += (_, e) =>
            {
                e.Handled = true;
                core.Navigate(e.Uri);
            };

            // Fallback, ha a primary URL tényleg nem tölt be
            core.NavigationCompleted += (_, e) =>
            {
                if (!e.IsSuccess)
                {
                    var url = WebView.Source?.ToString() ?? "";
                    if (url.StartsWith(PrimaryUrl, StringComparison.OrdinalIgnoreCase))
                    {
                        core.Navigate(FallbackUrl);
                    }
                }
            };

            core.Settings.AreDefaultContextMenusEnabled = false;
            core.Settings.AreDevToolsEnabled = false;
            core.Settings.IsStatusBarEnabled = false;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!AllowClose)
            {
                e.Cancel = true;
                Hide();
                return;
            }

            base.OnClosing(e);
        }
    }
}

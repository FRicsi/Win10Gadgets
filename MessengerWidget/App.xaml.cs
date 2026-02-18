using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MessengerWidget
{
    public partial class App : Application
    {
        private TaskbarIcon? _trayIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Crash log (publishnál is hasznos) - LocalAppData alá
            var logDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MessengerWidget");
            Directory.CreateDirectory(logDir);
            var logPath = Path.Combine(logDir, "crash.log");

            AppDomain.CurrentDomain.UnhandledException += (_, ev) =>
            {
                File.AppendAllText(logPath, ev.ExceptionObject?.ToString() + Environment.NewLine);
            };

            DispatcherUnhandledException += (_, ev) =>
            {
                File.AppendAllText(logPath, ev.Exception.ToString() + Environment.NewLine);
                ev.Handled = true;
            };

            _trayIcon = new TaskbarIcon
            {
                Icon = LoadTrayIcon(),
                ToolTipText = "Messenger Widget"
            };

            _trayIcon.TrayLeftMouseDown += (_, __) =>
            {
                ToggleMainWindow();
            };

            _trayIcon.ContextMenu = BuildContextMenu();

            // A StartupUri miatt a MainWindow már létrejön; biztos ami biztos:
            if (MainWindow != null)
            {
                MainWindow.Show();
                MainWindow.Activate();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _trayIcon?.Dispose();
            _trayIcon = null;
            base.OnExit(e);
        }

        private ContextMenu BuildContextMenu()
        {
            var menu = new ContextMenu();

            var showItem = new MenuItem { Header = "Megjelenítés" };
            showItem.Click += (_, __) => ShowMainWindow();

            var hideItem = new MenuItem { Header = "Elrejtés" };
            hideItem.Click += (_, __) => HideMainWindow();

            var exitItem = new MenuItem { Header = "Kilépés" };
            exitItem.Click += (_, __) =>
            {
                if (MainWindow is MainWindow mw)
                {
                    mw.AllowClose = true;
                    mw.Close();
                }

                Shutdown();
            };

            menu.Items.Add(showItem);
            menu.Items.Add(hideItem);
            menu.Items.Add(new Separator());
            menu.Items.Add(exitItem);

            return menu;
        }

        private void ToggleMainWindow()
        {
            if (MainWindow == null)
            {
                MainWindow = new MainWindow();
            }

            if (MainWindow.IsVisible)
                HideMainWindow();
            else
                ShowMainWindow();
        }

        private void ShowMainWindow()
        {
            if (MainWindow == null)
            {
                MainWindow = new MainWindow();
            }

            MainWindow.Show();
            MainWindow.WindowState = WindowState.Normal;
            MainWindow.Activate();
        }

        private void HideMainWindow()
        {
            MainWindow?.Hide();
        }

        private static Icon LoadTrayIcon()
        {
            // Embedded resource: MessengerWidget.Assets.icon.ico
            const string resourceName = "MessengerWidget.Assets.icon.ico";

            var asm = Assembly.GetExecutingAssembly();
            using var stream = asm.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                return new Icon(stream);
            }

            // Fallback: futtatási mappából (dev convenience)
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "icon.ico");
            if (File.Exists(filePath))
            {
                return new Icon(filePath);
            }

            // Utolsó mentsvár: üres default icon (ne haljon meg az app)
            return SystemIcons.Application;
        }
    }
}

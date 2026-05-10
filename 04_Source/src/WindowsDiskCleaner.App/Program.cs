using System;
using System.Windows;
using System.Windows.Threading;

namespace WindowsDiskCleaner.App
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            StartupDiagnostics.Write("Program.Main entered.");

            try
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                {
                    StartupDiagnostics.Write("UnhandledException");
                    StartupDiagnostics.Write(args.ExceptionObject as Exception ?? new Exception(args.ExceptionObject.ToString()));
                };

                var app = new App();
                app.DispatcherUnhandledException += OnDispatcherUnhandledException;
                app.Startup += (sender, args) => StartupDiagnostics.Write("Application startup event.");
                app.Exit += (sender, args) => StartupDiagnostics.Write("Application exit event. ExitCode=" + args.ApplicationExitCode);

                var window = new MainWindow();
                StartupDiagnostics.Write("MainWindow constructed.");
                app.Run(window);
            }
            catch (Exception exception)
            {
                StartupDiagnostics.Write("Startup exception");
                StartupDiagnostics.Write(exception);
                MessageBox.Show(exception.ToString(), "Windows Disk Cleaner startup error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            StartupDiagnostics.Write("DispatcherUnhandledException");
            StartupDiagnostics.Write(args.Exception);
            MessageBox.Show(args.Exception.ToString(), "Windows Disk Cleaner runtime error", MessageBoxButton.OK, MessageBoxImage.Error);
            args.Handled = true;
        }
    }
}

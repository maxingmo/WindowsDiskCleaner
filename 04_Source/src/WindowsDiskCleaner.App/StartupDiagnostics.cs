using System;
using System.IO;

namespace WindowsDiskCleaner.App
{
    internal static class StartupDiagnostics
    {
        private static readonly object Sync = new object();

        public static void Write(string message)
        {
            try
            {
                var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                Directory.CreateDirectory(logDir);
                var logPath = Path.Combine(logDir, "startup.log");

                lock (Sync)
                {
                    File.AppendAllText(logPath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " " + message + Environment.NewLine);
                }
            }
            catch
            {
            }
        }

        public static void Write(Exception exception)
        {
            Write(exception.ToString());
        }
    }
}


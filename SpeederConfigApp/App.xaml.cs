using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using SpeederConfigApp.Tests;

namespace SpeederConfigApp
{
    public partial class App : Application
    {
        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Contains("--test") || e.Args.Contains("-t"))
            {
                AttachConsole(ATTACH_PARENT_PROCESS);

                int exitCode = SelfTestRunner.RunAllTests();
                Environment.Exit(exitCode);
                return;
            }

            base.OnStartup(e);
        }
    }
}

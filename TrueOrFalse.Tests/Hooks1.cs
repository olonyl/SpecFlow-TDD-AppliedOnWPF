using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;
using TestStack.White;

namespace TrueOrFalse.Tests
{
    [Binding]
    public sealed class Hooks1
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario]
        public void BeforeScenario()
        {
            KillRunningApp();

            var appFullPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "..\\..\\..\\bin\\Debug\\TrueOrFalse.exe"));

            var app = Application.Launch(appFullPath);
            Windows.SetApp(app);
        }

        private void KillRunningApp()
        {
            Process process = Process.GetProcessesByName("TrueOrFalse").FirstOrDefault();
            if (process == null)
                return;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    process.Kill();
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            KillRunningApp();
        }
    }
}

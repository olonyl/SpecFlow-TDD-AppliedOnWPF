using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TrueOrFalse.Tests.WinWrappers;

namespace TrueOrFalse.Tests
{
    public class Windows
    {
        private static Application _app;
        public static void SetApp(Application app)
        {
            _app = app;
        }

        public static MainWindow MainWindow => new MainWindow(_app.GetWindow("TrueOrFalse"));

        public static GameWindow GameWindow => new GameWindow(MainWindow.GameWindow());
    }
}

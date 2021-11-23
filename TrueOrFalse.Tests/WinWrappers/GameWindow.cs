using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace TrueOrFalse.Tests.WinWrappers
{
    public class GameWindow
    {
        private readonly Window _window;

        public GameWindow(Window window)
        {
            _window = window;
        }

        public void False()
        {
            _window.Get<Button>(SearchCriteria.ByText("False")).Click();
        }
        public void True()
        {
            _window.Get<Button>(SearchCriteria.ByText("True")).Click();
        }

        public string GetResult()
        {
            var modalWindow = _window.ModalWindow("Result");
            Label lbl = (Label) modalWindow.Get(SearchCriteria.ByControlType(ControlType.Text));
            return lbl.Text;
        }
    }
}

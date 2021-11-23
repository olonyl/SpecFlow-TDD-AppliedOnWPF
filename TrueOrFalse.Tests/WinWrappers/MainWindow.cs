using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TrueOrFalse.Models;

namespace TrueOrFalse.Tests.WinWrappers
{
    public class MainWindow
    {
        private Window _window;

        public MainWindow(Window window)
        {
            _window = window;
        }

        public Window GameWindow()
        {
            return _window.ModalWindow("Game");
        }

        public void StartNewGame()
        {
            _window.MenuBar.MenuItem("File", "Start Game").Click();
        }

        public void CutStatementToClipboard()
        {
            _window.MenuBar.MenuItem("Edit", "Cut").Click();
        }

        public void PreviousStatement()
        {
            _window.Get<Button>("PART_DecreaseButton").Click();
        }

        public void NextStatement()
        {
            _window.Get<Button>("PART_IncreaseButton").Click();
        }

        private int CurrentStatementNumber()
        {
            var tb = _window.Get<TextBox>();
            return int.Parse(tb.Text);
        }

        public void RemoveCurrentStatement()
        {
            _window.Get<Button>(SearchCriteria.ByText("Remove")).Click();
        }

        public Statement CurrentStatement()
        {
            string text = GetStatementTextBox().Text;
            bool flag = GetStatementCheckBox().Checked;
            return new Statement(text, flag);
        }

        public void AddStatement(Statement statement)
        {
            ChangeCurrentStatement(statement);
            ClickAddButton();
        }

        public void SaveCurrentStatement()
        {
            _window.Get<Button>(SearchCriteria.ByText("Save")).Click();
        }

        public void ChangeCurrentStatement(Statement statement)
        {
            SetStatementText(statement.Text);
            SetStatementTrueOrFalse(statement.IsTrue);
        }

        private TextBox GetStatementTextBox()
        {
            return _window.Get<TextBox>("CurrentStatement");
        }

        private CheckBox GetStatementCheckBox()
        {
            return _window.Get<CheckBox>("StatementFlag");
        }

        private void SetStatementText(string text)
        {
            GetStatementTextBox().Text = text;
        }

        private void SetStatementTrueOrFalse(bool flag)
        {
            GetStatementCheckBox().Checked = flag;
        }

        private void ClickAddButton()
        {
            _window.Get<Button>(SearchCriteria.ByText("Add")).Click();
        }

        public void GoToFirstStatement()
        {
            for (int i = CurrentStatementNumber(); i > 1; i--)
            {
                PreviousStatement();
            }
        }

        public int GetNumberOfStatements()
        {
            int numberOfStatements = 0;
            GoToFirstStatement();
            while (!string.IsNullOrWhiteSpace(CurrentStatement().Text))
            {
                NextStatement();
                numberOfStatements++;
            }

            return numberOfStatements;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TrueOrFalse.Models;
using TrueOrFalse.Tests.WinWrappers;

namespace TrueOrFalse.Tests.AcceptanceTests
{
    [Binding]
    public class EditingSteps
    {
        private List<Statement> _statements;

        [Given(@"I have five statements")]
        public void GivenIHaveFiveStatements()
        {
            _statements = new List<Statement>()
            {
                new Statement("elephant is big", true),
                new Statement("mouse is small", true),
                new Statement("java is better than C#", false),
                new Statement(".NET Core is cross-platform", true),
                new Statement("byte equals 4 bits", false),
            };
        }

        [Given(@"I added one statement")]
        public void GivenIAddedOneStatement()
        {
            Windows.MainWindow.AddStatement(_statements[0]);
        }

        [Given(@"Current Statement is Not Empty")]
        public void GivenCurrentStatementIsNotEmpty()
        {
            //enough to get to the first
            Windows.MainWindow.GoToFirstStatement();
        }

        [Given(@"I added two statements")]
        public void GivenIAddedTwoStatements()
        {
            Windows.MainWindow.AddStatement(_statements[0]);
            Windows.MainWindow.AddStatement(_statements[1]);
        }

        [When(@"I add one statement")]
        public void WhenIAddOneStatement()
        {
            Windows.MainWindow.AddStatement(_statements[0]);
        }

        [When(@"I Edit both text and statement's flag")]
        public void WhenIEditBothTextAndStatementSFlag()
        {
            var newStatement = new Statement("Changed", false);
            Windows.MainWindow.ChangeCurrentStatement(newStatement);
        }

        [When(@"Save the Editings")]
        public void WhenSaveTheEditings()
        {
            Windows.MainWindow.SaveCurrentStatement();
        }
        
        [When(@"I remove one of them")]
        public void WhenIRemoveOneOfThem()
        {
            Windows.MainWindow.GoToFirstStatement();
            Windows.MainWindow.RemoveCurrentStatement();
        }
        
        [When(@"I cut the statement's text")]
        public void WhenICutTheStatementSText()
        {
            Windows.MainWindow.CutStatementToClipboard();
        }
        
        [Then(@"it gets saved and I can get back to it")]
        public void ThenItGetsSavedAndICanGetBackToIt()
        {
            Windows.MainWindow.PreviousStatement();

            var statement = Windows.MainWindow.CurrentStatement();
            var addedStatement = _statements[0];

            Assert.AreEqual(statement, addedStatement);
        }
        
        [Then(@"It gets changed")]
        public void ThenItGetsChanged()
        {
            Statement changingStatement = _statements[0];

            Windows.MainWindow.PreviousStatement();
            Statement statement = Windows.MainWindow.CurrentStatement();

            Assert.AreNotEqual(statement, changingStatement);
        }
        
        [Then(@"Only one statement remains in the list")]
        public void ThenOnlyOneStatementRemainsInTheList()
        {
            Assert.AreEqual(1, Windows.MainWindow.GetNumberOfStatements());
        }
        
        [Then(@"It gets removed from the UI and saved into buffer")]
        public void ThenItGetsRemovedFromTheUIAndSavedIntoBuffer()
        {
            Statement statement = Windows.MainWindow.CurrentStatement();

            Assert.IsEmpty(statement.Text);
            Assert.AreEqual(_statements[0].Text, Clipboard.GetText());
        }
    }
}

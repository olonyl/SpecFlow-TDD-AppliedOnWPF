using System;
using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TrueOrFalse.Models;

namespace TrueOrFalse.Tests.AcceptanceTests
{
    [Binding]
    public class PlayingGameSteps
    {
        private List<Statement> _statements;

        [Given(@"I added five statements")]
        public void GivenIAddedFiveStatements()
        {
            _statements = new List<Statement>()
            {
                new Statement("elephant is big", true),
                new Statement("mouse is small", true),
                new Statement("java is better than C#", false),
                new Statement(".NET Core is cross-platform", true),
                new Statement("byte equals 4 bits", false),
            };
            foreach (var statement in _statements)
            {
                Windows.MainWindow.AddStatement(statement);
            }
        }
        
        [When(@"I give five answers right")]
        public void WhenIGiveFiveAnswersRight()
        {
            Windows.MainWindow.StartNewGame();
            foreach (var st in _statements)
            {
                if (st.IsTrue)
                    Windows.GameWindow.True();
                else
                    Windows.GameWindow.False();
            }
        }
        
        [Then(@"I win the game")]
        public void ThenIWinTheGame()
        {
            Assert.AreEqual("Win", Windows.GameWindow.GetResult());
        }
    }
}

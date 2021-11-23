using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TrueOrFalse.Models;
using TrueOrFalse.ViewModels;

namespace TrueOrFalse.Tests.ViewModelTests
{
    [TestFixture]
    public class GameViewModelTests
    {
        private List<Statement> _statements;
        private DialogServiceMock _dialogService;
        private GameViewModel _vm;

        [SetUp]
        public void Init()
        {
            _statements = new List<Statement>
            {
                new Statement("elephant is big", true),
                new Statement("mouse is small", true),
                new Statement("java is better than C#", false),
                new Statement(".net core is cross-platform", true),
                new Statement("byte equals 4 bits", false)
            };

            _dialogService = new DialogServiceMock();

            _vm = new GameViewModel(_statements, _dialogService);
        }

        [Test]
        public void Ctor_DefaultState()
        {
            Assert.AreEqual(1, _vm.StatementNumber);
            Assert.AreEqual(0, _vm.Score);
            Assert.AreEqual(_statements[0].Text, _vm.StatementText);
            Assert.AreSame(_statements[0], _vm.CurrentStatement);
        }

        [Test]
        public void CorrectAnswer_SetsCorrectState()
        {
            _vm.True();

            Assert.AreEqual(1, _vm.Score);
            Assert.AreEqual(2, _vm.StatementNumber);
            Assert.AreSame(_statements[1], _vm.CurrentStatement);
        }

        [Test]
        public void IncorrectAnswer_ScoreDoesntChange()
        {
            _vm.False();

            Assert.AreEqual(0, _vm.Score);
        }

        [TestCase(6, GameResult.Loss)]
        [TestCase(7, GameResult.Loss)]
        [TestCase(8, GameResult.Win)]
        public void GetResult_TenStatements_ReturnsCorrectResult(int scores, GameResult expectedResult)
        {
            var result = _vm.GetResult(scores, 10);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void EndOfGame_Win_OpensWindowWithCorrectMessage()
        {
            GiveCorrectReplies();

            Assert.IsTrue(_vm.EndOfGame());
            Assert.AreEqual("Win", _dialogService.InfoText);
        }

        [Test]
        public void EndOfGame_Lost_OpensWindowWithCorrectMessage()
        {
            GiveIncorrectReplies();

            Assert.IsTrue(_vm.EndOfGame());
            Assert.AreEqual("Loss", _dialogService.InfoText);
        }

        private void GiveIncorrectReplies()
        {
            foreach (var st in _statements)
            {
                if (st.IsTrue)
                    _vm.False();
                else
                    _vm.True();
            }
        }

        private void GiveCorrectReplies()
        {
            foreach (var st in _statements)
            {
                if(st.IsTrue)
                    _vm.True();
                else                
                    _vm.False();                
            }
        }
    }

}

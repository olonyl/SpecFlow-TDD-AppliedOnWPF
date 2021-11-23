using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TrueOrFalse.Models;
using TrueOrFalse.ViewModels;

namespace TrueOrFalse.Tests.ViewModelTests
{
    public class MainViewModelBuilder
    {
        private IPersistence _persistence;
        private IDialogService _dialog;
        private IWindowManager _windowManager;

        public MainViewModelBuilder WithPersistence(IPersistence persistence)
        {
            _persistence = persistence;
            return this;
        }

        public MainViewModelBuilder WithDialogService(IDialogService dialog)
        {
            _dialog = dialog;
            return this;
        }

        public MainViewModel Build()
        {
            return new MainViewModel(_persistence, _dialog, _windowManager);
        }

        public MainViewModelBuilder DependenciesNoMatter()
        {
            return new MainViewModelBuilder()
                .WithPersistence(new InMemoryPersistence())
                .WithDialogService(new DialogServiceMock())
                .WithWindowManager(new WindowManagerMock());
        }

        public MainViewModelBuilder WithWindowManager(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            return this;
        }
    }

    [TestFixture]
    public class MainViewModelTests
    {
        private InMemoryPersistence FilledPersistence()
        {
            var persist = new InMemoryPersistence();
            persist.Add(new Statement("elephant is big", true));
            persist.Add(new Statement("mouse is small", true));
            persist.Add(new Statement("java is better than C#", false));
            persist.Add(new Statement(".net core is cross-platform", true));
            persist.Add(new Statement("byte equals 4 bits", false));

            return persist;
        }

        [Test]
        public void Ctor_CorrectDefaultState()
        {
            MainViewModel vm = new MainViewModelBuilder()
                                    .WithPersistence(new InMemoryPersistence())
                                    .Build();

            Assert.IsNotNull(vm.CurrentStatement);
            Assert.AreEqual(1, vm.CurrentNumber);
            Assert.IsTrue(vm.IsStatementEmpty);
        }

        [TestCase(1, true)]
        [TestCase(6, false)]
        public void CanSaveAndRemoveStatement_ReturnsExpectedResult(int statementNumber, bool expectedResult)
        {
            MainViewModel vm = new MainViewModelBuilder()
                .WithPersistence(FilledPersistence())
                .Build();
            
            vm.CurrentNumber = statementNumber;

            Assert.AreEqual(expectedResult, vm.CanSaveStatement);
            Assert.AreEqual(expectedResult, vm.CanRemoveStatement);
        }

        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("NotEmpty", true)]
        public void CanSaveStatement_ChangeCurrentStatement_ReturnsExpectedResult(string statement, bool expectedResult)
        {
            MainViewModel vm = new MainViewModelBuilder()
                .WithPersistence(FilledPersistence())
                .Build();

            vm.CurrentNumber = 1;
            vm.CurrentStatement.Text = statement;

            Assert.AreEqual(expectedResult, vm.CanSaveStatement);
        }

        [TestCase(1, "text doesn't matter", false)]
        [TestCase(6, "some text", true)]
        [TestCase(6, "", false)]
        [TestCase(6, " ", false)]        
        public void CanAddStatement_ReturnsExpectedResult(int statementNumber, string text, bool expectedResult)
        {
            MainViewModel vm = new MainViewModelBuilder()
                .WithPersistence(FilledPersistence())
                .Build();

            vm.CurrentNumber = statementNumber;
            vm.CurrentStatement.Text = text;

            Assert.AreEqual(expectedResult, vm.CanAddStatement);
        }

        [Test]
        public void Save_SaveOnPersistenceWasCalled()
        {
            var persist = new InMemoryPersistence();
            MainViewModel vm = new MainViewModelBuilder()
                .WithPersistence(persist)
                .Build();

            vm.SaveDb();

            Assert.IsTrue(persist.SaveWasCalled);
        }

        [Test]
        public void SaveDbAs_DialogReturnsTrue_SaveCalledOnPersistence()
        {
            var dialogMock = new DialogServiceMock();
            var persist = new InMemoryPersistence();

            var vm = new MainViewModelBuilder()
                            .WithPersistence(persist)
                            .WithDialogService(dialogMock)
                            .Build();
            vm.SaveDbAs();

            Assert.IsTrue(persist.SaveWasCalled);
            Assert.IsTrue(dialogMock.SaveFileDialogWasCalled);
        }

        [TestCase("Text")]
        [TestCase("")]
        public void CutShouldCopyToClipboardAndCutSomeInputText(string text)
        {
            var vm = new MainViewModelBuilder()
                            .DependenciesNoMatter()
                            .Build();
            vm.CurrentStatement.Text = text;

            vm.Cut();

            Assert.AreEqual(text, Clipboard.GetText());
            Assert.IsEmpty(vm.CurrentStatement.Text);
        }

        [TestCase("Text")]
        [TestCase("")]
        public void PasteShouldPasteSomeInputTextToCurrentStatement(string text)
        {
            var vm = new MainViewModelBuilder()
                .DependenciesNoMatter()
                .Build();

            Clipboard.SetText(text);

            vm.Paste();

            Assert.AreEqual(text, vm.CurrentStatement.Text);
        }

        [TestCase("Text")]
        [TestCase("")]
        public void CopyShouldCopyCurrentStatementToClipboard(string text)
        {
            var vm = new MainViewModelBuilder()
                .DependenciesNoMatter()
                .Build();
            vm.CurrentStatement.Text = text;

            vm.Copy();

            Assert.AreEqual(text, Clipboard.GetText());
        }

        [Test]
        public void OpenDb_DialogReturnsTrue_SaveCalledOnPersistence()
        {
            var dialogMock = new DialogServiceMock();
            var persist = new InMemoryPersistence();

            var vm = new MainViewModelBuilder()
                .WithPersistence(persist)
                .WithDialogService(dialogMock)
                .Build();

            vm.OpenDb();

            Assert.IsTrue(persist.LoadWasCalled);
            Assert.AreEqual(1, vm.CurrentNumber);
        }

        [Test]
        public void NewDb_DialogReturnsTrue_SaveCalledOnPersistenceAndVmIsInCorrectState()
        {
            var dialog = new DialogServiceMock();
            var persist = new InMemoryPersistence();

            var vm = new MainViewModelBuilder()
                .WithPersistence(persist)
                .WithDialogService(dialog)
                .Build();

            vm.NewDb();

            Assert.IsTrue(persist.SaveWasCalled);
            Assert.IsTrue(dialog.SaveFileDialogWasCalled);

            Assert.AreEqual(1, vm.CurrentNumber);
            Assert.IsTrue(vm.IsStatementEmpty);
            Assert.IsFalse(vm.CurrentStatement.IsTrue);
        }

        [Test]
        public void AddStatement_SomeStatement_AddsToPersistence()
        {
            var persist = new InMemoryPersistence();

            var vm = new MainViewModelBuilder()
                            .WithPersistence(persist)
                            .Build();
            vm.CurrentStatement.Text = "Text";

            vm.AddStatement();

            Assert.AreEqual(2, vm.CurrentNumber);
            Assert.AreEqual(1, persist.Count);

            Assert.IsEmpty(vm.CurrentStatement.Text);
            Assert.IsFalse(vm.CurrentStatement.IsTrue);
        }

        [Test]
        public void RemoveStatement_RemovesFromPersistenceAndDecrementsCurrentNumber()
        {
            var persist = FilledPersistence();
            var vm = new MainViewModelBuilder()
                .WithPersistence(persist)
                .Build();
            vm.CurrentNumber = 5;

            vm.RemoveStatement();

            Assert.AreEqual(4, persist.Count);
            Assert.AreEqual(4, vm.CurrentNumber);

            vm.RemoveStatement();
            vm.RemoveStatement();
            vm.RemoveStatement();
            vm.RemoveStatement();

            Assert.AreEqual(0, persist.Count);
            Assert.AreEqual(1, vm.CurrentNumber);
        }

        [Test]
        public void SaveStatement_UpdatesPersistenceAndIncrementsCurrentNumber()
        {
            var persist = FilledPersistence();
            var vm = new MainViewModelBuilder()
                .WithPersistence(persist)
                .Build();

            const string newText = "NextText";
            vm.CurrentStatement.Text = newText;

            vm.SaveStatement();

            Assert.AreEqual(newText, persist[0].Text);
            Assert.AreEqual(2, vm.CurrentNumber);
        }

        [Test]
        public void StartGame_OpensUpGameViewModel()
        {
            var winMock = new WindowManagerMock();

            var vm = new MainViewModelBuilder()
                        .WithPersistence(FilledPersistence())
                        .WithWindowManager(winMock)
                        .Build();

            vm.StartGame();

            Assert.IsInstanceOf<GameViewModel>(winMock.PassedRootModel);
        }
    }

    public class WindowManagerMock : IWindowManager
    {
        public object PassedRootModel { get; set; }
        public bool? ShowDialog(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {
            PassedRootModel = rootModel;
            return true;
        }

        public void ShowWindow(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {
            PassedRootModel = rootModel;           
        }

        public void ShowPopup(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {
            PassedRootModel = rootModel;
        }
    }

    public class DialogServiceMock : IDialogService
    {
        public DialogResult SaveFileDialog()
        {
            SaveFileDialogWasCalled = true;
            return new DialogResult(true, "");
        }

        public bool SaveFileDialogWasCalled { get; private set; }
        public bool OpenFileDialogWasCalled { get; private set; }

        public DialogResult OpenFileDialog()
        {
            OpenFileDialogWasCalled = true;
            return new DialogResult(true, "");
        }

        public void OpenInfoWindow(string caption, string text)
        {
            InfoText = text;
        }

        public string InfoText { get; private set; }
    }

    public class InMemoryPersistence:IPersistence
    {
        public int Count => List.Count;
        public List<Statement> List { get; } = new List<Statement>();
        public string FileName { get; set; }

        public void Add(Statement statement)
        {
            List.Add(statement);
        }

        public void Remove(int index)
        {
            List.RemoveAt(index);
        }

        public Statement this[int index] => List[index];

        public bool SaveWasCalled { get; private set; }
        public bool LoadWasCalled { get; private set; }

        public void Save()
        {
            SaveWasCalled = true;
        }

        public void Load()
        {
            LoadWasCalled = true;
        }

        public void Change(int index, Statement statement)
        {
            List[index] = statement;
        }

        public bool Exists(int index)
        {
            return List.Count > index;
        }
    }
}

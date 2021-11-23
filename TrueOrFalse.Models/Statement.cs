using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrueOrFalse.Models.Annotations;

namespace TrueOrFalse.Models {
    [Serializable]
    public class Statement : INotifyPropertyChanged {
        private string _text;
        private bool _isTrue;

        public Statement() { }

        public Statement(string text, bool isTrue) {
            Text = text;
            IsTrue = isTrue;
        }

        public string Text {
            get { return _text; }
            set {
                _text = value;
                OnPropertyChanged();
            }
        }

        public bool IsTrue {
            get { return _isTrue; }
            set {
                _isTrue = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Statement))
                return false;

            var statement = (Statement)obj;
            return Text == statement.Text && IsTrue == statement.IsTrue;
        }
    }
}
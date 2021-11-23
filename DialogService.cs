using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using TrueOrFalse.ViewModels;

namespace TrueOrFalse.Views
{
    public class DialogService : IDialogService
    {
        public DialogResult SaveFileDialog()
        {
            var sfd = new SaveFileDialog();
            var result = sfd.ShowDialog();

            return new DialogResult(result, result == true ? sfd.FileName : null);
        }

        public DialogResult OpenFileDialog()
        {
            var ofd = new OpenFileDialog();
            ofd.InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var result = ofd.ShowDialog();

            return new DialogResult(result, result == true ? ofd.FileName : null);
        }

        public void OpenInfoWindow(string caption, string text)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

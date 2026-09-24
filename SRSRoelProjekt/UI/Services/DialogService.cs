// SRSRoelProjekt/UI/Services/DialogService.cs
using System.Windows;
using SRSRoelProjekt.Core.Services;           // interface
using SRSRoelProjekt.ViewModels.Dialogs;     // ConfirmDialogViewModel
using SRSRoelProjekt.Views.Windows;          // ConfirmDialog

namespace SRSRoelProjekt.UI.Services
{
    public class DialogService : IDialogService
    {
        public bool ShowConfirm(string message)
        {
            var vm = new ConfirmDialogViewModel(message);

            // sæt CloseAction så ViewModel kan lukke sit vindue uden code-behind
            vm.CloseAction = () =>
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.DataContext == vm)
                    {
                        w.Close();
                        break;
                    }
                }
            };

            var dialog = new ConfirmDialog
            {
                DataContext = vm,
                Owner = Application.Current.MainWindow
            };

            dialog.ShowDialog();
            return vm.Result;
        }


        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Besked", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        public void CloseDialog(object viewModel)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.DataContext == viewModel)
                {
                    w.Close();
                    break;
                }
            }
        }
    }
}

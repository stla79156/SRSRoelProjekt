// SRSRoelProjekt/ViewModels/Dialogs/ConfirmDialogViewModel.cs
using System;
using System.Windows.Input;
using SRSRoelProjekt.Commands;

namespace SRSRoelProjekt.ViewModels.Dialogs
{
    public class ConfirmDialogViewModel
    {
        public string Message { get; }
        public bool Result { get; private set; }
        public Action CloseAction { get; set; }

        public ICommand YesCommand { get; }
        public ICommand NoCommand { get; }

        public ConfirmDialogViewModel(string message)
        {
            Message = message;
            YesCommand = new RelayCommand(() => Close(true));
            NoCommand = new RelayCommand(() => Close(false));
        }

        private void Close(bool result)
        {
            Result = result;
            CloseAction?.Invoke();
        }
    }
}

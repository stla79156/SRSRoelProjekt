using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Windows.Input;
using SRSRoelProjekt.Commands;

namespace SRSRoelProjekt.ViewModels.Dialogs
{
    public class InfoDialogViewModel : ViewModelBase
    {

        public string Message { get; }

        public Action CloseAction { get; set; }

        public ICommand OkCommand { get; }

        public InfoDialogViewModel(string message)
        {
            Message = message;
            OkCommand = new RelayCommand(OK);


        }

        private void OK()
        {
            CloseAction?.Invoke();
        
        }

    }
}

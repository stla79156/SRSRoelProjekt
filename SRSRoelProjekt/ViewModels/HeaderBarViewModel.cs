using SRSRoelProjekt.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SRSRoelProjekt.ViewModels
{
    public class HeaderBarViewModel : ViewModelBase
    {
        public RelayCommand LogOutCommand { get; }
        public HeaderBarViewModel()
        {
            LogOutCommand = new RelayCommand(LogOut);
        }

        private void LogOut()
        {
            Application.Current.Shutdown();
        }
    }
}

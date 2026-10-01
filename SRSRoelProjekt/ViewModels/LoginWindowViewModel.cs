using SRSRoelProjekt.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using SRSRoelProjekt;
using System.Windows.Input;

namespace SRSRoelProjekt.ViewModels
{
    public class LoginWindowViewModel
    {
        public RelayCommand LoginCommand { get; }
        public LoginWindowViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            Window mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}

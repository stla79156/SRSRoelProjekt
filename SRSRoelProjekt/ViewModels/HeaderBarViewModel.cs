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
            var result = MessageBox.Show(
                "Er du sikker på at du vil logge af?",
                "Bekræft log af",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
                return;

            // Find nuværende vindue
            var currentWindow = Application.Current.MainWindow;

            // Åbn login-vinduet
            var loginWindow = new MainWindow();
            loginWindow.Show();

            // Luk nuværende vindue
            currentWindow.Close();
            Application.Current.Shutdown(); //  Application.Current.Shutdown(); skal ersttes med  new LoginWindow().Show(); når login er implementeret
        }




    }
}

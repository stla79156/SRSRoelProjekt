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
        public RelayCommand OpenNewMenuCommand { get; }
        public HeaderBarViewModel()
        {
            OpenNewMenuCommand = new RelayCommand(OpenMenu);
        }

        private void OpenMenu()
        {
            Window MainWindow = new MainWindow();
            MainWindow.Show();
        }
    }
}

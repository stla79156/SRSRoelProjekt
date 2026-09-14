using SRSRoelProjekt.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace SRSRoelProjekt.ViewModels
{
    public class HeaderBarViewModel:ViewModelBase
    {

        public RelayCommand MenuCommand { get; }
        private readonly MainViewModel _main;

        public HeaderBarViewModel(MainViewModel main)
        {
            _main = main;
            MenuCommand = new RelayCommand(OpenMenu);
        }

        private void OpenMenu()
        {
            _main.ShowMenu();
        }


    }
}

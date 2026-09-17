using SRSRoelProjekt.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SRSRoelProjekt.ViewModels
{
    public class HeaderBarViewModel:ViewModelBase
    {


        private readonly MainViewModel _main;
        public RelayCommand ShowMenuCommand { get; }
       


        public HeaderBarViewModel(MainViewModel main)
        {
            _main = main;
            ShowMenuCommand = new RelayCommand(() => _main.ShowMenu());
        }
      

           
            
        



    }
}

using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SRSRoelProjekt.ViewModels
{
    public  class RackControlViewModel: ViewModelBase
    {

        private MainViewModel _main;
        public ObservableCollection<Renter> Renters { get; }
        public ICommand AddRenterCommand { get; }



        public RackControlViewModel(ObservableCollection<Renter> renters, MainViewModel main)
        {
            Renters = renters;
            _main = main;

            // Åbn popup-vinduet
            AddRenterCommand = new RelayCommand(OpenAddRenterWindow);
        }

        private void OpenAddRenterWindow()
        {
            var win = new AddRenterWindow();
            win.ShowDialog();
        }
    }












    
}

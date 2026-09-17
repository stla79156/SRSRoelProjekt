using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Views;
using SRSRoelProjekt.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SRSRoelProjekt.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
       public HeaderBarViewModel HeaderBarViewModel { get; }



        //public ObservableCollection<Renter> Renters { get; set; }

        //public RelayCommand OpenAddRenterCommand { get; }

        public ObservableCollection<Renter> Renters { get; }

        public ViewModelBase CurrentViewModel { get; set; }

        public ICommand ShowRackControlCommand { get; }
        public ICommand ShowAddRenterCommand { get; }

        public MainViewModel()
        {
            Renters = new ObservableCollection<Renter>();

            ShowRackControlCommand = new RelayCommand(ShowRackControl);
            ShowAddRenterCommand = new RelayCommand(ShowAddRenter);

            ShowRackControl(); // Start view
        }

        public void ShowRackControl()
        {
            CurrentViewModel = new RackControlViewModel(Renters, this);
        }

        public void ShowAddRenter()
        {
            CurrentViewModel = new AddRenterViewModel(Renters, this);
        }

        public void ShowMenu()
        {
            CurrentViewModel = new HeaderBarViewModel(this);
        }









        /* public void ShowRackControl()
         {
             CurrentViewModel = new RackControlViewModel(Renters, this);
         }*/



    }










}

using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Windows;
using SRSRoelProjekt.Views;
using SRSRoelProjekt.Views.Windows;

namespace SRSRoelProjekt.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
       public HeaderBarViewModel HeaderBarViewModel { get; }
        public ViewModelBase CurrentViewModel { get; set; }

        public void ShowMenu()
        {
            //CurrentViewModel = new MenuViewModel(); Kan først efter oprettelse af flere views til at refere til. 
        }


        public ObservableCollection<Renter> Renters { get; set; }

        public RelayCommand OpenAddRenterCommand { get; }

        public MainViewModel()
        {
            Renters = new ObservableCollection<Renter>();
            OpenAddRenterCommand = new RelayCommand(OpenAddRenterWindow);
        }

        private void OpenAddRenterWindow()
        {
            AddRenterWindow win = new AddRenterWindow();
            win.Owner = Application.Current.MainWindow;

            // ViewModel til vinduet
            var vm = new AddRenterViewModel(Renters);
            win.DataContext = vm;

            win.ShowDialog();
        }


        


    }










}

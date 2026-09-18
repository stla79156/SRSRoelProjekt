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
        private Renter _selectedRenter;

        public ObservableCollection<Renter> Renters { get; }

        public ICommand AddRenterCommand { get; }
        public ICommand StopRentalCommand { get; }
        public ICommand SaveRackCommand { get; }
        public ICommand RemoveRenterCommand { get; }

        

        public RackControlViewModel(ObservableCollection<Renter> renters, MainViewModel main)
        {
            Renters = renters;
            _main = main;

            // Åbn popup-vinduet
            AddRenterCommand = new RelayCommand(OpenAddRenterWindow);
            StopRentalCommand = new RelayCommand(StopRental);
            SaveRackCommand = new RelayCommand(SaveRack);
            RemoveRenterCommand = new RelayCommand(RemoveRenter); 

        }


        


        public Renter SelectedRenter
        {
            get => _selectedRenter;
            set
            {
                _selectedRenter = value;
                OnPropertyChanged();
                HighlightRenterShelves();
            }
        }




        private void OpenAddRenterWindow()
        {
            var win = new AddRenterWindow(_main.Renters, _main);
            win.ShowDialog();
        }

        private void StopRental()
        {

            if (SelectedRenter == null)
            {
                MessageBox.Show("Vælg en lejer først");
                return;

            }
            _main.FloorPlanViewModel.StopRentalForRenter(
            SelectedRenter.Name,
            DateTime.Today.AddMonths(1)
        );


        }
        private void RemoveRenter()
        {
            if (SelectedRenter == null)
            {
                MessageBox.Show("Vælg en lejer først");
                return; 

            }

            _main.FloorPlanViewModel.ClearRenterRack(SelectedRenter.Name);
            Renters.Remove(SelectedRenter);
            SelectedRenter = null;
        }



        private void SaveRack()
        {
            if (SelectedRenter == null)
            {
                MessageBox.Show("Vælg en lejer først.");
                return;
            }

            _main.FloorPlanViewModel.SaveReservation(SelectedRenter.Name);
        }



        private void HighlightRenterShelves()
        {
            if (SelectedRenter == null)
                return;

            _main.FloorPlanViewModel.HighlightRenterShelves(SelectedRenter.Name);
        }

       



        
    }













}

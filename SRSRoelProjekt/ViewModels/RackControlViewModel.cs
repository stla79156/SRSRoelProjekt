using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Views;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
                _main.SelectedRenter = value;
                OnPropertyChanged();
                HighlightRenterShelves();

                bool showTooltips = _selectedRenter == null;

                foreach (var rack in _main.FloorPlanViewModel.Racks)
                {
                    rack.CanShowInfo = showTooltips;
                }
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

            else
            {
                foreach (var rack in _main.FloorPlanViewModel.Racks)
                {
                    if (rack.RenterName == SelectedRenter.Name)
                    { 
                        MessageBox.Show("Denne lejer har stadig reserverede reoler.");
                        return;
                    }
                }
                var result = MessageBox.Show("Er du sikker på at du vil fjerne denne lejer?", "Bekræft fjerning", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                    return;
            }

            var renter = SelectedRenter;
            // Fjern reol-reservationer
            _main.FloorPlanViewModel.ClearRenterRack(SelectedRenter.Name);

            // ⭐ GEM I JSON
            _main.RenterService.RemoveRenter(SelectedRenter.RenterId);

            // Fjern fra UI-listen
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

        public void ClearSelection()
        {
            SelectedRenter = null;

            foreach (var rack in _main.FloorPlanViewModel.Racks)
            {
                if (rack.Status == RackStatus.Selected)
                {
                    rack.Status = RackStatus.Available;
                }
                rack.IsHighlighted = false;
            }
            
        }

        private void HighlightRenterShelves()
        {
            if (SelectedRenter == null)
                return;

            _main.FloorPlanViewModel.HighlightRenterShelves(SelectedRenter.Name);
        }

        

    }













}

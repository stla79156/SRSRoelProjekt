using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Services;
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

        private readonly IDialogService _dialogService;
        public ObservableCollection<Renter> Renters { get; }

        public ICommand AddRenterCommand { get; }
        public ICommand StopRentalCommand { get; }
        public ICommand SaveRackCommand { get; }
        public ICommand RemoveRenterCommand { get; }

        

        public RackControlViewModel(ObservableCollection<Renter> renters, MainViewModel main, IDialogService dialogService)
        {
            Renters = renters;
            _main = main;
            _dialogService = dialogService;

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
            }
        }




        private void OpenAddRenterWindow()
        {

            bool ok = _dialogService.ShowConfirm("Vil du tilføje en ny lejer?");
            if (!ok) return;

            var win = new AddRenterWindow(_main.Renters, _main);
            win.ShowDialog();
        }

        private void StopRental()
        {
            if (SelectedRenter == null)
            {
                _dialogService.ShowMessage("Vælg en lejer først.");
                return;
            }
            else
            if (_main.FloorPlanViewModel.SelectedRack == null)
            {
                _dialogService.ShowMessage("Vælg en reol først.");
                return;
            }

            bool confirm = _dialogService.ShowConfirm(
                $"Vil du fjerne reol {_main.FloorPlanViewModel.SelectedRack.RackNumber} fra lejer '{SelectedRenter.Name}'?"
            );

            if (!confirm)
                return;

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
                // Tjek om lejeren stadig har reserverede reoler
                foreach (var rack in _main.FloorPlanViewModel.Racks)
                {
                    if (rack.RenterName == SelectedRenter.Name)
                    {
                        // Informér brugeren (her bruger vi dialogService til en simpel besked via ShowConfirm med kun OK)
                        _dialogService.ShowMessage("Denne lejer har stadig reserverede reoler.");
                        return;
                    }
                }
            }


            bool confirm = _dialogService.ShowConfirm($"Er du sikker på at du vil fjerne lejer '{SelectedRenter.Name}'?");
            if (!confirm) return;


            var renter = SelectedRenter;
            // Fjern reol-reservationer
            _main.FloorPlanViewModel.ClearRenterRack(SelectedRenter.Name);

            // ⭐ GEM I JSON
            //_main.RenterService.RemoveRenter(SelectedRenter.RenterId);

            // Fjern fra UI-listen
            Renters.Remove(SelectedRenter);


           
            SelectedRenter = null;

            _dialogService.ShowMessage("Lejer er nu blevet fjernet.");
        }



        private void SaveRack()
        {
            if (SelectedRenter == null)
            {
                MessageBox.Show("Vælg en lejer først.");
                return;
            }
            else
            if (_main.FloorPlanViewModel.SelectedRack == null)
            {
                _dialogService.ShowMessage("Vælg en reol først.");
                return;
            }

            bool confirm = _dialogService.ShowConfirm(
                $"Vil du tilføje reol {_main.FloorPlanViewModel.SelectedRack.RackNumber} til lejer '{SelectedRenter.Name}'?"
            );

            if (!confirm)
                return;

            _main.FloorPlanViewModel.SaveReservation(SelectedRenter.Name);

            ClearSelection();
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

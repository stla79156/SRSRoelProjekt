using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Services;
using SRSRoelProjekt.Core.Repositories;
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

        private readonly IRackRepository _rackRepo;

        private readonly IDialogService _dialogService;
        public ObservableCollection<Renter> Renters { get; }

        public ICommand AddRenterCommand { get; }
        public ICommand StopRentalCommand { get; }
        public ICommand SaveRackCommand { get; }
        public ICommand RemoveRenterCommand { get; }

        

        public RackControlViewModel(ObservableCollection<Renter> renters, MainViewModel main, IDialogService dialogService, IRackRepository rackRepo)
        {
            Renters = renters;
            _main = main;
            _rackRepo = rackRepo;
            _dialogService = dialogService;

            // Åbn popup-vinduet
            AddRenterCommand = new RelayCommand(OpenAddRenterWindow);
            StopRentalCommand = new RelayCommand(StopRentalForRenter);
            SaveRackCommand = new RelayCommand(StartRentalForRenter);
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

            bool ok = _dialogService.ShowConfirm("Vil du tilføje en ny lejer?");
            if (!ok) return;

            var win = new AddRenterWindow(_main.Renters, _main);
            win.ShowDialog();
        }

        /*private void StopRental()
        {
            if (SelectedRenter == null)
            {
                _dialogService.ShowMessage("Vælg en lejer først.");
                return;
            }
            else
            if (_main.RackViewModel.IsSelected == false)
            {
                _dialogService.ShowMessage("Vælg en reol først.");
                return;
            }

            bool confirm = _dialogService.ShowConfirm(
                $"Vil du fjerne reol {_main.RackViewModel.RackNumber} fra lejer '{SelectedRenter.Name}'?"
            );

            if (!confirm)
                return;
        }*/

        public void StopRentalForRenter()
        {
            if(SelectedRenter == null)
            {
                _dialogService.ShowMessage("Vælg en lejer først.");
                return;
            }

            var racksToStop = _main.FloorPlanViewModel.Racks
                .Where(r =>
                    r.IsSelected &&
                    r.Status == RackStatus.Reserved &&
                    r.RenterId == SelectedRenter.RenterId)
                .ToList();

            if (!racksToStop.Any())
            {
                _dialogService.ShowMessage("Vælg en reol først.");
                return;
            }

            foreach (var rack in racksToStop)
            {
                _rackRepo.StopRental(rack.RackNumber);

                rack.Status = RackStatus.EndingSoon;
                rack.IsSelected = false;
            }

            _main.FloorPlanViewModel.Racks.Clear();
            _main.FloorPlanViewModel.CreateRackLayout();

            ClearSelection();
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



        private void StartRentalForRenter()
        {
            if (SelectedRenter == null)
            {
                _dialogService.ShowMessage("Vælg en lejer først.");
                return;
            }

            var selectedRacks = _main.FloorPlanViewModel.Racks.Where(r => r.IsSelected).ToList();

            if (!selectedRacks.Any())
            {
                _dialogService.ShowMessage("Vælg en reol først.");
                return;
            }

            string rackNumbers = string.Join(", ", selectedRacks.Select(r => r.RackNumber));

            bool confirm = _dialogService.ShowConfirm(
                $"Vil du tilføje reol(er) {rackNumbers} til lejer '{SelectedRenter.Name}'?"
            );

            if (!confirm)
                return;

            foreach (var rack in selectedRacks)
            {
                _rackRepo.StartRental(rack.RackNumber, SelectedRenter.RenterId);

                rack.Status = RackStatus.Reserved;
            }
            _main.FloorPlanViewModel.Racks.Clear();
            _main.FloorPlanViewModel.CreateRackLayout();

            ClearSelection();
        }

        public void ClearSelection()
        {
            SelectedRenter = null;

            foreach (var rack in _main.FloorPlanViewModel.Racks)
            {
                if (rack.IsSelected)
                {
                    rack.IsSelected = false;
                }
                rack.IsHighlighted = false;
            }
            
        }

        private void HighlightRenterShelves()
        {
            if (SelectedRenter == null)
                return;

            _main.FloorPlanViewModel.HighlightRenterShelves(SelectedRenter.RenterId);
        }

        

    }













}

using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Repositories;
using SRSRoelProjekt.Core.Repositories.SRSRoelProjekt.Core.Repositories;
using SRSRoelProjekt.Core.Services;
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

        public RackControlViewModel RackControlViewModel { get; }

        public FloorPlanViewModel FloorPlanViewModel { get; }

        public RenterService RenterService { get; }

        private Renter _selectedRenter;
        public Renter SelectedRenter
        {
            get => _selectedRenter;
            set
            {
                if (_selectedRenter != value)
                {
                    _selectedRenter = value;
                    OnPropertyChanged();

                    FloorPlanViewModel.ClearSelectedRacks();
                }
            }
        }

        //public ObservableCollection<Renter> Renters { get; set; }

        //public RelayCommand OpenAddRenterCommand { get; }

        public ObservableCollection<Renter> Renters { get; }

        public ViewModelBase CurrentViewModel { get; set; }

        public ICommand ShowRackControlCommand { get; }
        public ICommand ShowAddRenterCommand { get; }



    
        public MainViewModel()

        {
            var repo = new SqlRenterRepository();
            RenterService = new RenterService(repo);

            // Load renters from JSON FIRST
            Renters = RenterService.GetRenters();

            FloorPlanViewModel = new FloorPlanViewModel(this);

            RackControlViewModel =
                new RackControlViewModel(
                    Renters,
                    this);
            HeaderBarViewModel =
                new HeaderBarViewModel();
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
            CurrentViewModel = new HeaderBarViewModel();
        }









       



    }










}

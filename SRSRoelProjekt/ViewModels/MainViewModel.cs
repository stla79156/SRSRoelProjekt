using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Repositories;
using SRSRoelProjekt.Views;
using SRSRoelProjekt.Views.UserControls;
using SRSRoelProjekt.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SRSRoelProjekt.Core.Services;
using SRSRoelProjekt.UI.Services; // DialogService-implementering
using SRSRoelProjekt.ViewModels;

namespace SRSRoelProjekt.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
         // public HeaderBarViewModel HeaderBarViewModel { get; }
        private readonly IRackRepository _rackRepo;
        private readonly IRenterRepository _renterRepo;
        public HeaderBarViewModel HeaderBarViewModel { get; }
      

        public RackControlViewModel RackControlViewModel { get; }

        public RackViewModel RackViewModel { get; }

        public FloorPlanViewModel FloorPlanViewModel { get; }

        public RenterService RenterService { get; }

        private Renter _selectedRenter;

        public Renter SelectedRenter
        {
            get => _selectedRenter;
            set
            {
                if (_selectedRenter == value)
                    return;

                _selectedRenter = value;

                OnPropertyChanged();

                FloorPlanViewModel.ClearSelectedRacks();

                if (_selectedRenter != null)
                {
                    FloorPlanViewModel.HighlightRenterShelves(
                        _selectedRenter.RenterId);
                }
                else
                {
                    FloorPlanViewModel.ClearHighlightedRacks();
                }

                FloorPlanViewModel.UpdateTooltips();
            }
        }

        //public ObservableCollection<Renter> Renters { get; set; }

        //public RelayCommand OpenAddRenterCommand { get; }

        public ObservableCollection<Renter> Renters { get; }

        public ViewModelBase CurrentViewModel { get; set; }



        public ICommand ShowRackControlCommand { get; }
        public ICommand ShowAddRenterCommand { get; }

        private readonly IDialogService _dialogService;


        public MainViewModel()
        {
            _dialogService = new DialogService();

            _renterRepo = new SqlRenterRepository();
            _rackRepo = new SQLRackRepository();

            RenterService = new RenterService(_renterRepo);

            Renters = RenterService.GetRenters();

            FloorPlanViewModel = new FloorPlanViewModel(this, _rackRepo);

            RackViewModel = new RackViewModel();

            HeaderBarViewModel = new HeaderBarViewModel(_dialogService);

            RackControlViewModel = new RackControlViewModel(
                Renters,
                this,
                _dialogService,
                _rackRepo,
                _renterRepo);

            CurrentViewModel = FloorPlanViewModel;
        }

        public void ShowRackControl()
        {
            CurrentViewModel = new RackControlViewModel(Renters, this, _dialogService, _rackRepo, _renterRepo);
        }

        public void ShowAddRenter()
        {
            CurrentViewModel = new AddRenterViewModel(Renters, this);
        }

        public void ShowMenu()
        {
            CurrentViewModel = new HeaderBarViewModel(new SRSRoelProjekt.UI.Services.DialogService());
        }









       



    }










}

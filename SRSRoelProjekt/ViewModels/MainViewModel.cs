using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Repositories;
using SRSRoelProjekt.Core.Repositories.SRSRoelProjekt.Core.Repositories;

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

        private readonly IDialogService _dialogService;


        public MainViewModel()

        {

            // opret én DialogService og genbrug den
            _dialogService = new SRSRoelProjekt.UI.Services.DialogService();

            var repo = new SqlRenterRepository();
            var rackRepo = new SQLRackRepository();
            RenterService = new RenterService(repo);

            // Load renters from Database
            Renters = RenterService.GetRenters();

            FloorPlanViewModel = new FloorPlanViewModel(this, new SQLRackRepository());

            RackViewModel = new RackViewModel();

            //RackControlViewModel = new RackControlViewModel(Renters, this);

            // Opret DialogService-implementeringen fra UI-laget
            // Sørg for at SRSRoelProjekt (WPF) har reference til SRSRoelProjekt.Core
            // IDialogService dialogService = new SRSRoelProjekt.UI.Services.DialogService();


            // Opret HeaderBarViewModel med dialogService
            //HeaderBarViewModel = new HeaderBarViewModel(dialogService);
            //RackControlViewModel = new RackControlViewModel(Renters, this, dialogService);
            HeaderBarViewModel = new HeaderBarViewModel(_dialogService);
            RackControlViewModel = new RackControlViewModel(Renters, this, _dialogService, rackRepo);

            // sæt start-ViewModel hvis nødvendigt
            CurrentViewModel = FloorPlanViewModel;
        }

        public void ShowRackControl()
        {
            CurrentViewModel = new RackControlViewModel(Renters, this, _dialogService, _rackRepo);
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

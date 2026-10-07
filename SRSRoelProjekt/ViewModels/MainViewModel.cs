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
using SRSRoelProjekt.UI.Services;
using SRSRoelProjekt.ViewModels;
namespace SRSRoelProjekt.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IRackRepository _rackRepo;
        private readonly IRenterRepository _renterRepo;
        public RackControlViewModel RackControlViewModel { get; }
        public FloorPlanViewModel FloorPlanViewModel { get; }
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
            Renters = new ObservableCollection<Renter>(_renterRepo.GetRenters());

            FloorPlanViewModel = new FloorPlanViewModel(this, _rackRepo);

            RackControlViewModel = new RackControlViewModel(Renters, this, _dialogService, _rackRepo, _renterRepo);

            CurrentViewModel = FloorPlanViewModel;

        }

        public void ShowRackControl()
        {
            CurrentViewModel = new RackControlViewModel(Renters, this, _dialogService, _rackRepo, _renterRepo);
        }

    }

}


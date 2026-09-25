using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Repositories;
using SRSRoelProjekt.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace SRSRoelProjekt.ViewModels
{
    public class FloorPlanViewModel : ViewModelBase
    {
        private readonly IRackRepository _rackRepository;


        public ObservableCollection<RackViewModel> Racks { get; } = new();

        public RelayCommand RackClickedCommand { get; }

        private MainViewModel _main;

        private Rack _selectedRack;
        public Rack SelectedRack
        {
            get => _selectedRack;
            set
            {
                _selectedRack = value;
                OnPropertyChanged();
            }
        }

        public FloorPlanViewModel(MainViewModel main, IRackRepository rackRepository)
        {
            _main = main;
            _rackRepository = rackRepository;

            RackClickedCommand =
                new RelayCommand(OnRackClicked);

            CreateRackLayout();

            //click for info on racks becomes true. this disables when a renter is selected in the combo box.
            foreach (var rack in Racks)
            {
                rack.CanShowInfo = true;
            }
        }

        private void OnRackClicked(object parameter)
        {
            
            if (parameter is not RackViewModel rack)
                return;

            // No renter selected -> show info window
            if (_main.SelectedRenter == null)
            {
                ShowRackInfo(rack);
                return;
            }

            if(rack.Status == RackStatus.Reserved && _main.SelectedRenter.RenterId != rack.RenterId)
            {
                return;
            }

            // Reserved racks cannot be changed
            if (rack.Status == RackStatus.EndingSoon)
            {
                return;
            }


            rack.IsSelected = true;

            Debug.WriteLine($"CLICKED: {rack.RackNumber} - {rack.IsSelected} - {rack.RenterName}");


        }

        private void ShowRackInfo(RackViewModel rack)
        {
            var window = new RackInfoWindow(rack);
            window.ShowDialog();
        }
        public void CreateRackLayout()
        {
            var dbRacks = _rackRepository.GetRacks().ToDictionary(r => r.RackNumber);
            bool[,] rackLayout =
            {
                { false,false,false,false,false,true,true,false,true,true,false,true,true,false,false,false,false,false,false,false },
                { false,false,false,false,false,true,true,false,true,true,false,true,true,false,false,false,false,false,false,false },
                { false,false,false,false,false,true,true,false,true,true,false,true,true,false,true,true,false,false,false,false },
                { true,false,false,false,false,true,true,false,true,true,false,true,true,false,true,true,false,false,false,false },
                { true,false,true,true,false,true,true,false,true,true,false,true,true,false,true,true,false,true,true,false },
                { true,false,true,true,false,true,true,false,true,true,false,true,true,false,true,true,false,false,false,false },
                { true,false,true,true,false,true,true,false,true,true,false,true,true,false,true,true,false,true,true,false },
                { true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false },
                { false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false },
                { false,false,true,true,true,true,true,true,true,true,true,true,true,true,true,false,false,false,false,false }
            };

            int rackNumber = 1;

            for (int row = 0; row < rackLayout.GetLength(0); row++)
            {
                for (int col = 0; col < rackLayout.GetLength(1); col++)
                {
                    if (rackLayout[row, col])
                    {
                        var dbRack = dbRacks[rackNumber];

                        Racks.Add(new RackViewModel
                        {
                            RackNumber = rackNumber,
                            WithHanger = dbRack.WithHanger,
                            Status = dbRack.RackStatus,
                            EndDate = dbRack.EndDate,
                            AvailableFrom = dbRack.AvailableFrom,
                            RenterId = dbRack.RenterId,
                            IsVisible = true,
                            
                        });

                        rackNumber++;
                    }
                    else
                    {
                        Racks.Add(new RackViewModel
                        {
                            IsVisible = false
                        });
                    }
                }
            }
        }

        //Asign racks with hangers based on their rack numbers
        /*private readonly HashSet<int> racksWithHangers =
        [
            21, 30, 41, 42, 43, 54, 67, 68, 69, 70, 
            71, 72, 73, 74, 75, 76, 77, 78, 79, 80
        ];*/

        public void HighlightRenterShelves(int renterId)
        {
            foreach (var rack in Racks)
            {
                rack.IsHighlighted = false;
            }

            foreach (var rack in Racks.Where(x => x.RenterId == renterId))
            {
                rack.IsHighlighted = true;
            }
        }
        public void ClearSelectedRacks()
        {
            foreach (var rack in Racks.Where(r => r.IsSelected))
            {
                rack.IsSelected = false;
            }
        }
        public void SaveReservation(RackViewModel selectedRack, string renterName)
        {
            selectedRack.RenterName = renterName;
            selectedRack.Status = RackStatus.Reserved;
        }

        private DateTime CalculateAvailableFrom(DateTime endDate)
        {
            if (endDate.Day < 20)
            {
                return new DateTime(
                    endDate.Year,
                    endDate.Month,
                    1).AddMonths(1);
            }

            return new DateTime(
                endDate.Year,
                endDate.Month,
                1).AddMonths(2);
        }

        

        public void ClearRenterRack(string renterName)
        {
            foreach (var rack in Racks.Where(x => x.RenterName == renterName))
            {
                rack.RenterName = null;
                rack.Status = RackStatus.Available;
                rack.IsHighlighted = false;
            }
        }


    }


   
}
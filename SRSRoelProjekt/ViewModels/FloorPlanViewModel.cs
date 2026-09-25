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
            UpdateTooltips();

            //click for info on racks becomes true. this disables when a renter is selected in the combo box.
            /*foreach (var rack in Racks)
            {
                rack.Tool = true;
            }*/

            



        }

        private void OnRackClicked(object parameter)
        {
            if (parameter is not RackViewModel rack)
                return;

            if (_main.SelectedRenter == null)
            {
                ShowRackInfo(rack);
                return;
            }

            if (rack.Status == RackStatus.Reserved &&
            rack.RenterId != _main.SelectedRenter.RenterId)
            {
                return;
            }

            if (rack.Status == RackStatus.EndingSoon)
            {
                return;
            }

            rack.IsSelected = !rack.IsSelected;
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
                            RenterName = _main.Renters.FirstOrDefault(r => r.RenterId == dbRack.RenterId)?.Name,

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
                rack.IsHighlighted =
                    rack.RenterId == renterId &&
                    (
                        rack.Status == RackStatus.Reserved ||
                        rack.Status == RackStatus.EndingSoon
                    );
            }
        }

        public void ClearHighlightedRacks()
        {
            foreach (var rack in Racks)
            {
                rack.IsHighlighted = false;
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

        public void UpdateTooltips()
        {
            foreach (var rack in Racks)
            {
                if (_main.SelectedRenter == null)
                {
                    rack.TooltipText = "Klik for info";
                }
                else if (
                    rack.Status == RackStatus.Reserved &&
                    rack.RenterId != _main.SelectedRenter.RenterId)
                {
                    rack.TooltipText =
                        "Reolen er reserveret af en anden lejer";
                }
                else if(rack.Status == RackStatus.EndingSoon && rack.RenterId != _main.SelectedRenter.RenterId)
                {
                    rack.TooltipText = $"Reolen er ved at udløbe. Ledig igen: {rack.AvailableFrom:dd-MM-yyyy}";
                }
                else
                {
                    rack.TooltipText = null;
                }
            }
        }

        /*private bool _canShowInfo;

        public bool CanShowInfo
        {
            get => _canShowInfo;
            set
            {
                _canShowInfo = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TooltipText));
            }
        }*/




    }


   
}
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

        public FloorPlanViewModel(MainViewModel main, IRackRepository rackRepository)
        {
            _main = main;
            _rackRepository = rackRepository;

            RackClickedCommand =
                new RelayCommand(OnRackClicked);

            CreateRackLayout();
            UpdateTooltips();

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

    }
   
}
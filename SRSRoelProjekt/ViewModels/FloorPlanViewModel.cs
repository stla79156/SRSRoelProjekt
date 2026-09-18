using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SRSRoelProjekt.ViewModels
{
    public class FloorPlanViewModel : ViewModelBase
    {
        public ObservableCollection<RackViewModel> Racks
        {
            get;
        } = new();

        public RelayCommand RackClickedCommand { get; }

        private RackViewModel highlightedRack;

        public FloorPlanViewModel()
        {
            RackClickedCommand =
                new RelayCommand(OnRackClicked);

            CreateShelfLayout();
        }

        private void OnRackClicked(object parameter)
        {
            if (parameter is not RackViewModel rack)
                return;

            if (rack.Status == RackStatus.Reserved ||
                rack.Status == RackStatus.EndingSoon)
            {
                highlightedRack = rack;
                return;
            }

            if (rack.Status == RackStatus.Selected)
            {
                rack.Status = RackStatus.Available;
            }
            else
            {
                rack.Status = RackStatus.Selected;
            }
        }

        private void CreateShelfLayout()
        {
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
                        Racks.Add(new RackViewModel
                        {
                            RackNumber = rackNumber++,
                            Status = RackStatus.Available,
                            IsVisible = true
                        });
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

        public void HighlightRenterShelves(string renterName)
        {
            foreach (var rack in Racks)
            {
                rack.IsHighlighted = false;
            }

            foreach (var rack in Racks
                .Where(x => x.RenterName == renterName))
            {
                rack.IsHighlighted = true;
            }
        }

        public void SaveReservation(string renterName)
        {
            foreach (var rack in Racks
                .Where(x => x.Status == RackStatus.Selected))
            {
                rack.RenterName = renterName;
                rack.Status = RackStatus.Reserved;
            }
        }

        public void StopRentalForRenter(
            string renterName,
            DateTime endDate)
        {
            foreach (var rack in Racks
                .Where(x => x.RenterName == renterName))
            {
                rack.Status = RackStatus.EndingSoon;
            }
        }

        public void ClearRenterRack(string renterName)
        {
             
            foreach (var rack in Racks.Where (x => x.RenterName == renterName))
            {
                rack.RenterName = null;
                rack.Status = RackStatus.Available;
                rack.IsHighlighted = false;

            }
        
        
        }

    }
}
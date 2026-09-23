using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace SRSRoelProjekt.ViewModels
{
    public class FloorPlanViewModel : ViewModelBase
    {
        public ObservableCollection<RackViewModel> Racks { get; } = new();

        public RelayCommand RackClickedCommand { get; }

        private MainViewModel _main;

        public FloorPlanViewModel(MainViewModel main)
        {
            _main = main;

            RackClickedCommand =
                new RelayCommand(OnRackClicked);

            CreateRackLayout();
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

            // Reserved racks cannot be changed
            if (rack.Status == RackStatus.Reserved ||
                rack.Status == RackStatus.EndingSoon)
            {
                return;
            }

            rack.Status =
                rack.Status == RackStatus.Selected
                    ? RackStatus.Available
                    : RackStatus.Selected;
        }

        private void ShowRackInfo(RackViewModel rack)
        {
            var window = new RackInfoWindow(rack);
            window.ShowDialog();
        }
        private void CreateRackLayout()
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
                            RackNumber = rackNumber,
                            Status = RackStatus.Available,
                            IsVisible = true,
                            WithHanger = racksWithHangers.Contains(rackNumber)
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
        private readonly HashSet<int> racksWithHangers =
        [
            21, 30, 31, 43, 44, 54, 55, 67, 68, 69, 70, 
            71, 72, 73, 74, 75, 76, 77, 78, 79, 80
        ];

        public void HighlightRenterShelves(string renterName)
        {
            foreach (var rack in Racks)
            {
                rack.IsHighlighted = false;
            }

            foreach (var rack in Racks.Where(x => x.RenterName == renterName))
            {
                rack.IsHighlighted = true;
            }
        }
        public void ClearSelectedRacks()
        {
            foreach (var rack in Racks.Where(r => r.Status == RackStatus.Selected))
            {
                rack.Status = RackStatus.Available;
            }
        }
        public void SaveReservation(string renterName)
        {
            foreach (var rack in Racks.Where(x => x.Status == RackStatus.Selected))
            {
                rack.RenterName = renterName;
                rack.Status = RackStatus.Reserved;
            }
        }

        public void StopRentalForRenter(string renterName, DateTime endDate)
        {
            foreach (var rack in Racks.Where(x => x.RenterName == renterName))
            {
                rack.Status = RackStatus.EndingSoon;
            }
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


    public class RackViewModel : ViewModelBase
    {
        private RackStatus _status;
        private bool _isHighlighted;
        private bool _isVisible = true;

        public int RackNumber { get; set; }
        public string? RenterName { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? AvailableFrom { get; set; }

        public RackStatus Status
        {
            get => _status;
            set
            {
                _status = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }
        public bool WithHanger { get; set; }

        public string DisplayText => WithHanger
        ? $"{RackNumber}\nb"
        : RackNumber.ToString();


        public bool IsHighlighted
        {
            get => _isHighlighted;
            set
            {
                _isHighlighted = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(BorderBrush));
                OnPropertyChanged(nameof(BorderThickness));
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(RackVisibility));
            }
        }

        public Visibility RackVisibility =>
            IsVisible
                ? Visibility.Visible
                : Visibility.Hidden;

        public Brush BackgroundColor =>
            Status switch
            {
                RackStatus.Available => Brushes.LightGreen,
                RackStatus.Selected => Brushes.Blue,
                RackStatus.Reserved => Brushes.Red,
                RackStatus.EndingSoon => Brushes.Yellow,
                _ => Brushes.Gray
            };

        public Brush BorderBrush =>
            IsHighlighted
                ? Brushes.Blue
                : Brushes.Black;

        public double BorderThickness =>
            IsHighlighted ? 3 : 1;
    }
}
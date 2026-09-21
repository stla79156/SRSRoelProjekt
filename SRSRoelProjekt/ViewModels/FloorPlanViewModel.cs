using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Text;
using System.Collections.Generic;
using System.Windows.Media;



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

        private MainViewModel _main;

        public FloorPlanViewModel(MainViewModel main)
        {
            _main = main;
            RackClickedCommand =
                new RelayCommand(OnRackClicked);

            createRackLayout();
        }

        private void OnRackClicked(object parameter)
        {

            if (_main.SelectedRenter == null)
            {
                MessageBox.Show("Vælg en lejer først");
                return;
            }

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

        private void createRackLayout()
        {
            CreateShelfLayout();
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


        public Visibility RackVisibility =>
            IsVisible
                ? Visibility.Visible
                : Visibility.Hidden;

        public int RackNumber { get; set; }

        public string RenterName { get; set; }

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


        private bool _isVisible = true;

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







    }
}
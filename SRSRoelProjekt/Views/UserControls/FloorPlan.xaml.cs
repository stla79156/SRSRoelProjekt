using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Views.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SRSRoelProjekt.Views.UserControls
{
    /// <summary>
    /// Interaction logic for FloorPlan.xaml
    /// </summary>
    public partial class FloorPlan : UserControl
    {
        private List<Rack> selectedRack = new();
        private List<Button> selectedButtons = new();
        private Rack highlightedRack;


        private readonly Dictionary<Button, Rack> rackMap =
            new Dictionary<Button, Rack>();

        public string SelectedRenter { get; set; }
        public FloorPlan()
        {
            InitializeComponent();
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
        { false,false,true,true,true,true,true,true,true,true,true,true,true,true,true,false,false,false,false,false},

    };

            int rackNumber = 1;

            for (int row = 0; row < rackLayout.GetLength(0); row++)
            {
                for (int col = 0; col < rackLayout.GetLength(1); col++)
                {
                    Button box = new Button
                    {
                        Margin = new Thickness(1)
                    };

                    if (rackLayout[row, col])
                    {
                        box.Content = rackNumber.ToString();
                        box.Background = Brushes.LightGreen;

                        Rack rack = new Rack
                        {
                            RackNumber = rackNumber,
                            Status = RackStatus.Available
                        };

                        rackMap.Add(box, rack);

                        box.Click += Rack_Click;

                        rackNumber++;
                    }
                    else
                    {
                        box.Visibility = Visibility.Hidden;
                    }

                    StorageGrid.Children.Add(box);
                }
            }
        }
        public void HighlightRenterShelves(string selectedRenter)
        {
            foreach (var pair in rackMap)
            {
                Button button = pair.Key;
                Rack rack = pair.Value;

                // Reset border first
                button.BorderBrush = Brushes.Black;
                button.BorderThickness = new Thickness(1);

                // Highlight renter's shelves
                if (rack.RenterName == selectedRenter)
                {
                    button.BorderBrush = Brushes.Blue;
                    button.BorderThickness = new Thickness(3);
                }
            }
        }
        private void Rack_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)sender;

            Rack rack = rackMap[clicked];

            if (rack.Status == RackStatus.Reserved ||
                rack.Status == RackStatus.EndingSoon)
            {
                highlightedRack = rack;
                RackInfoWindow infoWindow =
               new RackInfoWindow(rack);
                infoWindow.ShowDialog();

                return;
            }


            // Already selected -> deselect
            if (selectedRack.Contains(rack))
            {
                selectedRack.Remove(rack);
                selectedButtons.Remove(clicked);

                rack.Status = RackStatus.Available;
                clicked.Background = Brushes.LightGreen;
                clicked.Foreground = Brushes.Black;

                return;
            }

            // Select
            selectedRack.Add(rack);
            selectedButtons.Add(clicked);

            rack.Status = RackStatus.Selected;
            clicked.Background = Brushes.Blue;
            clicked.Foreground = Brushes.White;
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

        public bool ConfirmReservation(string renterName)
        {
            string rackList = string.Join(
        "\n",
        selectedRack.Select(
            r => $"- Reol {r.RackNumber}"));

            string message =
                $"Lejer:\n{renterName}\n\n" +
                $"Reoler tilføjet til leje:\n{rackList}\n\n";
            var result = MessageBox.Show(
                message,
                "Accepter ændringer",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }
        public void SaveReservation(string renterName)
        {
            if (!ConfirmReservation(renterName))
            {
                return;
            }

            foreach (Rack rack in selectedRack)
            {
                rack.RenterName = renterName;
                rack.Status = RackStatus.Reserved;

                Button button =
                    rackMap.First(x => x.Value == rack).Key;

                button.Background = Brushes.Red;
            }

            selectedRack.Clear();
            selectedButtons.Clear();
        }


        public void StopRentalForRenter(string renterName, DateTime endDate)
        {

            if (highlightedRack == null)
                return;

            highlightedRack.Status = RackStatus.EndingSoon;

            highlightedRack.EndDate = endDate;

            highlightedRack.AvailableFrom =
                CalculateAvailableFrom(endDate);

            Button button =
                rackMap.First(x => x.Value == highlightedRack).Key;
            button.Background = Brushes.Yellow;
            button.Foreground = Brushes.Black;
        }


    }
}


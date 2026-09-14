using SRSRoelProjekt.Core.Models;
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
using System.Windows.Shapes;

namespace SRSRoelProjekt.Views.Windows
{
    /// <summary>
    /// Interaction logic for RackInfoWindow.xaml
    /// </summary>
    public partial class RackInfoWindow : Window
    {
        public RackInfoWindow(Rack rack)
        {
            InitializeComponent();

            ShelfNumberText.Text =
                $"Reol: {rack.RackNumber}";

            RenterText.Text =
                $"Lejer: {rack.RenterName ?? "Ingen"}";

            StatusText.Text =
                $"Status: {rack.Status}";

            EndDateText.Text =
                $"Slutdato: {(rack.EndDate?.ToShortDateString() ?? "N/A")}";

            AvailableFromText.Text =
            $"Ledig fra: {(rack.AvailableFrom?.ToShortDateString() ?? "N/A")}";

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

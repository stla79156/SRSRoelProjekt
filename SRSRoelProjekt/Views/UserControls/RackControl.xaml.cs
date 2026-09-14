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
using SRSRoelProjekt.Views.Windows;

namespace SRSRoelProjekt.Views.UserControls
{
    /// <summary>
    /// Interaction logic for RackControl.xaml
    /// </summary>
    public partial class RackControl : UserControl
    {
        public RackControl()
        {
            InitializeComponent();
            this.Loaded += RackControl_Loaded;
        }

        private void RackControl_Loaded(object? sender, RoutedEventArgs e)
        {
            // Ensure selection happens after the control tree (and MainWindow children) are created
            if (RenterComboBox != null && RenterComboBox.Items.Count > 0)
            {
                RenterComboBox.SelectedIndex = 0;
            }
            // detach handler to avoid re-running
            this.Loaded -= RackControl_Loaded;
        }
        private void SaveShelf_Click(object sender, RoutedEventArgs e)
        {
            string renterName =
                ((ComboBoxItem)RenterComboBox.SelectedItem)?
                .Content?
                .ToString();

            if (string.IsNullOrEmpty(renterName))
            {
                MessageBox.Show("Vælg en lejer først.");
                return;
            }

            ((MainWindow)Application.Current.MainWindow)
                .SaveShelfReservation(renterName);
        }

        private void StopRental_Click(object sender, RoutedEventArgs e)
        {
            string renterName =
                ((ComboBoxItem)RenterComboBox.SelectedItem)?
                .Content?
                .ToString();

            if (string.IsNullOrEmpty(renterName))
            {
                MessageBox.Show("Vælg en lejer først");
                return;
            }

            MainWindow mainWindow =
                (MainWindow)Application.Current.MainWindow;

            mainWindow.MyFloorPlanControl.StopRentalForRenter(renterName, DateTime.Today.AddMonths(1));
        }
        private void RenterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string renterName =
                ((ComboBoxItem)RenterComboBox.SelectedItem)?
                .Content?
                .ToString();

            if (string.IsNullOrEmpty(renterName))
                return;

            var mainWindow = Application.Current?.MainWindow as MainWindow;
            var floorPlan = mainWindow?.MyFloorPlanControl;
            if (floorPlan == null)
                return;

            floorPlan.HighlightRenterShelves(renterName);
        }
    }
}

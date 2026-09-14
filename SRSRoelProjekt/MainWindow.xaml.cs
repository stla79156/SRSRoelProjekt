using SRSRoelProjekt.Views.UserControls;
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

namespace SRSRoelProjekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public FloorPlan MyFloorPlanControl
        {
            get { return MyFloorPlan; }
        }
        public void SaveRackReservation(string renterName)
        {
            MyFloorPlan.SaveReservation(renterName);
        }

        private void MyFloorPlan_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}

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
            //this.Loaded += RackControl_Loaded;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

           

        }

        // snakker sammen med viewmodel

        private void RenterComboBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (RenterComboBox.IsDropDownOpen &&
                RenterComboBox.SelectedItem != null)
            {
                var item = e.OriginalSource as FrameworkElement;

                if (item?.DataContext == RenterComboBox.SelectedItem)
                {
                    RenterComboBox.SelectedItem = null;
                    e.Handled = true;
                }
            }
        }



    }
}

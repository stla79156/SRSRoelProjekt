using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.ViewModels;
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
                DataContext = new RackInfoWindowViewModel(rack);
            }

            private void Close_Click(object sender, RoutedEventArgs e)
            {
                Close();
            }
        
    }
}

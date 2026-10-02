using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SRSRoelProjekt.ViewModels;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Services;

namespace SRSRoelProjekt.Views.Windows
{
    /// <summary>
    /// Interaction logic for RenterWindow.xaml
    /// </summary>
    public partial class RenterWindow : Window
    {
        public RenterWindow(Renter renter)
        {
            InitializeComponent();
            DataContext = new RenterWindowViewModel(renter);
        }
    }
}

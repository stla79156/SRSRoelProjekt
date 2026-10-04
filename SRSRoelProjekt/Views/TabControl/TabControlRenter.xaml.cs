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

namespace SRSRoelProjekt.Views.TabControl
{
    /// <summary>
    /// Interaction logic for TabControlRenter.xaml
    /// </summary>
    public partial class TabControlRenter : Window
    {
        public TabControlRenter(Renter renter)
        {
            InitializeComponent();

            DataContext = new TabControlRenterViewModel(
            new RenterWindowViewModel(renter),
            new MonthlyStatementRenterViewModel(renter));
        }
        private bool _resizing;

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_resizing)
                return;

            _resizing = true;

            Height = Width / 1.6;

            _resizing = false;
        }
    }

}

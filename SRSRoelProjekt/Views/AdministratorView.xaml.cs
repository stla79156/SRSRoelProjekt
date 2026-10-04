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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SRSRoelProjekt.Views
{
    /// <summary>
    /// Interaction logic for AdministratorView.xaml
    /// </summary>
    public partial class AdministratorView : UserControl
    {
        public AdministratorView()
        {
            InitializeComponent();
        }

        private void Admin_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdministratorViewModel vm)
            {
                vm.IsAdmin = true;
            }
        }

        private void Employee_Checked(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdministratorViewModel vm)
            {
                vm.IsAdmin = false;
            }
        }
    }
}

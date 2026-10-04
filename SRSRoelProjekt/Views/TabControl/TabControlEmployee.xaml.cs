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
    /// Interaction logic for TabControlEmployee.xaml
    /// </summary>
    public partial class TabControlEmployee : Window
    {
        public TabControlEmployee()
        {
            InitializeComponent();

            DataContext = new TabControlEmployeeViewModel(
                new MainViewModel(),
                new RegisterViewModel(),
                new MonthlyStatementViewModel());
        }
    }
}

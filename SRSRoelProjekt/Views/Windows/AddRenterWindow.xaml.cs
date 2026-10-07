using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Repositories;
using SRSRoelProjekt.ViewModels;
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


namespace SRSRoelProjekt.Views
{
    /// <summary>
    /// Interaction logic for AddRenterWindow.xaml
    /// </summary>
    public partial class AddRenterWindow : Window
    {
        public AddRenterWindow(ObservableCollection<Renter> renters, MainViewModel main, IRenterRepository renterRepository)
        {
            InitializeComponent();

            var vm = new AddRenterViewModel(renters, main, renterRepository);

            vm.CloseAction = result =>
            {
                DialogResult = result;
                Close();
            };

            DataContext = vm;
        }
    }
}

using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Windows;

namespace SRSRoelProjekt.ViewModels
{
    public class AddRenterViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public RelayCommand AddRenterCommand { get; }
        public RelayCommand CancelCommand { get; }

        private ObservableCollection<Renter> _renters;
        private MainViewModel _main;





        public AddRenterViewModel(ObservableCollection<Renter> renters, MainViewModel main)
        {
            _renters = renters;
            _main = main;

            AddRenterCommand = new RelayCommand(AddRenter);
            CancelCommand = new RelayCommand(() => _main.ShowRackControl());
        }

        private void AddRenter()
        {
            var renter = new Renter
            {
                Id = _main.RenterService.GenerateNewId(_renters),
                Name = Name,
                Email = Email,
                PhoneNumber = PhoneNumber
            };

            _renters.Add(renter);

            _main.RenterService.SaveRenters(_renters);

            _main.ShowRackControl();
        }


    }
}

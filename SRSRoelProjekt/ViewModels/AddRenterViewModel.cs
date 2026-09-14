using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Linq;
using System.Windows;

namespace SRSRoelProjekt.ViewModels
{
    public class AddRenterViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public RelayCommand AddRenterCommand { get; }

        private ObservableCollection<Renter> _renters;

        public AddRenterViewModel(ObservableCollection<Renter> renters)
        {
            _renters = renters;
            AddRenterCommand = new RelayCommand(AddRenter);
        }

        private void AddRenter()
        {
            _renters.Add(new Renter
            {
                Name = this.Name,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber
            });

            // Luk vinduet
            Application.Current.Windows
                .OfType<AddRenterWindow>()
                .FirstOrDefault()?
                .Close();
        }
    }
}

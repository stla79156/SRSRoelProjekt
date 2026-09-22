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
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using System.Windows;

    public class AddRenterViewModel : ViewModelBase
    {
        private string _name = string.Empty;
        private string _email = string.Empty;
        private string _phoneNumber = string.Empty;

        private readonly ObservableCollection<Renter> _renters;
        private readonly MainViewModel _main;

        public Action<bool?>? CloseAction { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();

                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EmailError));

                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PhoneError));

                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string EmailError =>
            string.IsNullOrWhiteSpace(Email) || IsValidEmail(Email)
                ? string.Empty
                : "Email skal indeholde @ og .";

        public string PhoneError =>
            string.IsNullOrWhiteSpace(PhoneNumber) || IsValidPhoneNumber(PhoneNumber)
                ? string.Empty
                : "Telefonnummer skal være 8-15 cifre.";

        public RelayCommand AddRenterCommand { get; }
        public RelayCommand CancelCommand { get; }

        public AddRenterViewModel(
            ObservableCollection<Renter> renters,
            MainViewModel main)
        {
            _renters = renters;
            _main = main;

            AddRenterCommand =
                new RelayCommand(AddRenter, CanAddRenter);

            CancelCommand =
                new RelayCommand(() => _main.ShowRackControl());
        }

        private bool CanAddRenter()
        {
            return !string.IsNullOrWhiteSpace(Name)
                   && IsValidEmail(Email)
                   && IsValidPhoneNumber(PhoneNumber);
        }

        private bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email)
                   && email.Contains("@")
                   && email.Contains(".");
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrWhiteSpace(phoneNumber)
                   && phoneNumber.Length >= 8
                   && phoneNumber.Length <= 15
                   && phoneNumber.All(char.IsDigit);
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

            _main.RenterService.AddRenter(renter);

            _renters.Add(renter);

           // _main.RenterService.SaveRenters(_renters);

            CloseAction?.Invoke(true);
        }
    }
}


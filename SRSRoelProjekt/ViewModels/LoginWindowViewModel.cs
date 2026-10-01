using SRSRoelProjekt.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using SRSRoelProjekt;
using System.Windows.Input;
using SRSRoelProjekt.Core.Repositories;
using SRSRoelProjekt.Core.Models;
using System.Linq;
using SRSRoelProjekt.Core.Services;
using SRSRoelProjekt.Views.Windows;

namespace SRSRoelProjekt.ViewModels
{
    public class LoginWindowViewModel : ViewModelBase
    {
        private readonly SQLEmployeeRepository _employeeRepo;
        private readonly SqlRenterRepository _renterRepo;
        private readonly IDialogService _dialogService;

        private string _username;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(UsernameError));
            }
        }

        public string UsernameError =>
            string.IsNullOrWhiteSpace(Username) || IsValidUsername(Username)
                ? string.Empty
                : "Ugyldigt brugernavn";

        public Action<bool?>? CloseAction { get; set; }

        public ICommand LoginCommand { get; }

        public LoginWindowViewModel()
        {
            _employeeRepo = new SQLEmployeeRepository();
            _renterRepo = new SqlRenterRepository();
            LoginCommand = new RelayCommand(Login);
        }

        private void Login(object parameter)
        {
            if (IsEmployeeUsername(Username))
            {
                var employee = _employeeRepo.GetEmployeeByUsername(Username);
                if (employee == null)
                {
                    _dialogService.ShowMessage("Medarbejder ikke fundet.");
                    return;
                }
                else
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    CloseAction?.Invoke(true);
                }
            }
            if (IsRenterUsername(Username))
            {
                var renter = _renterRepo.GetRenterByUsername(Username);
                if (renter == null)
                {
                    _dialogService.ShowMessage("Lejer ikke fundet.");
                    return;
                }
                else
                {
                    var renterWindow = new RenterWindow();
                    renterWindow.Show();
                    CloseAction?.Invoke(true);
                }
            }
        }

        private bool IsEmployeeUsername(string username)
        {
            return !string.IsNullOrWhiteSpace(username)
            && char.IsDigit(username[0]);
        }

        private bool IsRenterUsername(string username)
        {
            return !string.IsNullOrWhiteSpace(username)
            && username.Length >= 2
            && char.IsLetter(username[0])
            && char.IsLetter(username[1]);
        }

        public bool IsValidUsername(string username)
        {
            if (IsEmployeeUsername(username))
            {
                return !string.IsNullOrWhiteSpace(username)
                       && username.Length == 6
                       && username.All(char.IsDigit);
            }
            if (IsRenterUsername(username)) 
            {
                return !string.IsNullOrWhiteSpace(username)
                   && username.Length == 6
                   && char.IsLetter(username[0])
                   && char.IsLetter(username[1])
                   && username.Substring(2).All(char.IsDigit);
            }
            return false;
        }
    }
}

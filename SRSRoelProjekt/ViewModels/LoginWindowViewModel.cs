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

namespace SRSRoelProjekt.ViewModels
{
    public class LoginWindowViewModel : ViewModelBase
    {


        private readonly SQLEmployeeRepository _employeeRepo;
        private readonly SqlRenterRepository _renterRepo;
        private string _userId;

        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged();
            }
        }

        private string _userIdError;

        public string UserIdError
        {
            get => _userIdError;
            set
            {
                _userIdError = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        public LoginWindowViewModel()
        {
            _employeeRepo = new SQLEmployeeRepository();
            _renterRepo = new SqlRenterRepository();
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            _employeeRepo.GetEmployeeByUsername(UserId);
        }
    }
}

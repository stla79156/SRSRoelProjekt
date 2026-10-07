using System;
using System.Collections.Generic;
using System.Text;
using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Repositories;
using SRSRoelProjekt.Core.Services;
using SRSRoelProjekt.UI.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SRSRoelProjekt.ViewModels
{
    public class AdministratorViewModel : ViewModelBase
    {
        private readonly SQLEmployeeRepository _employeeRepository;
        private readonly IDialogService _dialogService;

        public ObservableCollection<Employee> Employees { get; }

        private Employee _selectedEmployee;
            public Employee SelectedEmployee
            {
                get => _selectedEmployee;
                set
                {
                    _selectedEmployee = value;
                    OnPropertyChanged();
                }
            }

        private string _employeeName;
        public string EmployeeName
        {
            get => _employeeName;
            set
            {
                _employeeName = value;
                OnPropertyChanged();
            }
        }

        private string _employeeUserName;
        public string EmployeeUserName
        {
            get => _employeeUserName;
            set
            {
                _employeeUserName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EmployeeUserNameError));
            }
        }

        public string EmployeeUserNameError =>
            string.IsNullOrWhiteSpace(EmployeeUserName) || IsValidUsername(EmployeeUserName)
                ? string.Empty
                : "Ugyldigt brugernavn, skal bestå af præcis 6 tal \n" +
                  "Eksempel: 123456";

        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                OnPropertyChanged();
            }
        }

        private Employee _loggedInEmployee;

        public ICommand AddEmployeeCommand { get; }

        public ICommand RemoveEmployeeCommand { get; }

        public AdministratorViewModel(Employee employee)
        {
            _loggedInEmployee = employee;
            _employeeRepository = new SQLEmployeeRepository();
            _dialogService = new DialogService();

            Employees = new ObservableCollection<Employee>();

            IsAdmin = false;

            AddEmployeeCommand = new RelayCommand(AddEmployee);
            RemoveEmployeeCommand = new RelayCommand(RemoveEmployee);

            LoadEmployees();
        }

        private void LoadEmployees()
        {
            Employees.Clear();

            foreach (var employee in _employeeRepository.GetEmployees())
            {
                Employees.Add(employee);
            }
        }

        private void AddEmployee(object parameter)
        {

            if (_loggedInEmployee.IsAdmin == false)
            {
                _dialogService.ShowMessage("Du har ikke tilladelse til at tilføje medarbejdere.");
                return;
            }

            if (string.IsNullOrWhiteSpace(EmployeeName))
            {
                _dialogService.ShowMessage("Indtast et navn.");
                return;
            }

            if (EmployeeName.Any(char.IsDigit))
            {
                _dialogService.ShowMessage(
                "Navnet må ikke indeholde tal.");
                return;
            }

            if (string.IsNullOrWhiteSpace(EmployeeUserName))
            {
                _dialogService.ShowMessage("Indtast et brugernavn.");
                return;
            }

            if (!IsValidUsername(EmployeeUserName))
            {
                _dialogService.ShowMessage(
                "Medarbejderens brugernavn skal bestå af præcis 6 tal.");
                return;
            }

            if (_employeeRepository.GetEmployees()
                .Any(e => e.EmployeeUserName == EmployeeUserName))
            {
                _dialogService.ShowMessage(
                "Brugernavnet findes allerede.");
                return;
            }


            Employee employee = new Employee
            {
                EmployeeName = EmployeeName,
                EmployeeUserName = EmployeeUserName,
                IsAdmin = IsAdmin
            };

            _employeeRepository.AddEmployee(employee);

            EmployeeName = string.Empty;
            EmployeeUserName = string.Empty;
            IsAdmin = false;

            OnPropertyChanged(nameof(EmployeeName));
            OnPropertyChanged(nameof(EmployeeUserName));
            OnPropertyChanged(nameof(IsAdmin));

            LoadEmployees();

            _dialogService.ShowMessage("Medarbejder oprettet.");
        }

        private bool IsValidUsername(string username)
        {
            return username.Length == 6 &&
            username.All(char.IsDigit);
        }

        private void RemoveEmployee(object parameter)
        {
            if (SelectedEmployee == null)
            {
                _dialogService.ShowMessage("Vælg en medarbejder.");
                return;
            }

            if (_loggedInEmployee.IsAdmin == false)
            {
                _dialogService.ShowMessage("Du har ikke tilladelse til at fjerne medarbejdere.");
                return;
            }


            bool confirm = _dialogService.ShowConfirm(
                $"Er du sikker på at du vil fjerne {SelectedEmployee.EmployeeName}?");

            if (!confirm)
            {
                return;
            }

            _employeeRepository.RemoveEmployee(SelectedEmployee);

            LoadEmployees();

            _dialogService.ShowMessage("Medarbejder fjernet.");
        }
    }
}

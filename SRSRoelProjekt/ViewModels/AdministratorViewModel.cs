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

                EmployeeUserNameError = string.Empty;

                if (string.IsNullOrWhiteSpace(value))
                {
                    EmployeeUserNameError = string.Empty;
                }
                else if (value.Any(char.IsLetter))
                {
                    EmployeeUserNameError =
                    "Brugernavnet må kun indeholde præcis 6 tal";
                }
                else
                {
                    EmployeeUserNameError = string.Empty;
                }

                OnPropertyChanged();
            }
        }



      

        private string _employeeUserNameError;

        public string EmployeeUserNameError
        {
            get => _employeeUserNameError;
            set
            {
                _employeeUserNameError = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddEmployeeCommand { get; }

            public ICommand RemoveEmployeeCommand { get; }

            public AdministratorViewModel()
            {
                _employeeRepository = new SQLEmployeeRepository();
                _dialogService = new DialogService();

                Employees = new ObservableCollection<Employee>();

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

            string formattedName = EmployeeName;

           



            if (!string.IsNullOrWhiteSpace(EmployeeName))
            {
                formattedName =
                EmployeeName.Substring(0, 1).ToUpper() +
                EmployeeName.Substring(1).ToLower();
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


            if (EmployeeUserName.Length != 6 ||
                !EmployeeUserName.All(char.IsDigit))
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


            if (_employeeRepository.GetEmployees()
                 .Any(e => e.EmployeeUserName == EmployeeUserName))
            {
                _dialogService.ShowMessage(
                "Brugernavnet findes allerede.");

                return;
            }

            if (!IsValidUsername(EmployeeUserName))
            {
                _dialogService.ShowMessage(
                "Medarbejderens brugernavn skal bestå af præcis 6 tal.");

                return;
            }

            


            Employee employee = new Employee
                {
                    EmployeeName = EmployeeName,
                    EmployeeUserName = EmployeeUserName
                };

                _employeeRepository.AddEmployee(employee);

                EmployeeName = string.Empty;
                EmployeeUserName = string.Empty;

                OnPropertyChanged(nameof(EmployeeName));
                OnPropertyChanged(nameof(EmployeeUserName));

                LoadEmployees();

                _dialogService.ShowMessage("Medarbejder oprettet.");
            }

            private void RemoveEmployee(object parameter)
            {
                if (SelectedEmployee == null)
                {
                    _dialogService.ShowMessage("Vælg en medarbejder.");
                    return;
                }


            if (SelectedEmployee.IsAdministrator)
            {
                _dialogService.ShowMessage(
                "Du kan ikke slette en administrator.");

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


        private bool IsEmployeeUsername(string username)
        {
            return !string.IsNullOrWhiteSpace(username)
            && char.IsDigit(username[0]);
        }

        

        public bool IsValidUsername(string username)
        {
            if (IsEmployeeUsername(username))
            {
                return !string.IsNullOrWhiteSpace(username)
                       && username.Length == 6
                       && username.All(char.IsDigit);
            }
            return false;
        }







    }
    
}

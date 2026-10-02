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
                if (string.IsNullOrWhiteSpace(EmployeeName))
                {
                    _dialogService.ShowMessage("Indtast et navn.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(EmployeeUserName))
                {
                    _dialogService.ShowMessage("Indtast et brugernavn.");
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
        }
    
}

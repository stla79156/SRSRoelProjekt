using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Services;
using SRSRoelProjekt.UI.Services;
using System;
using System.Collections.Generic;
using System.Drawing.Interop;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;

namespace SRSRoelProjekt.ViewModels
{
    public class TabControlEmployeeViewModel : ViewModelBase
    {
        public MainViewModel MainViewModel { get; }
        public RegisterViewModel RegisterViewModel { get; }
        public MonthlyStatementViewModel MonthlyStatementViewModel { get; }
        public AdministratorViewModel AdministratorViewModel { get; }
        public RelayCommand LogOutCommand { get; }
        private readonly IDialogService _dialogService;
        public TabControlEmployeeViewModel(MainViewModel mainViewModel, RegisterViewModel registerViewModel, MonthlyStatementViewModel monthlyStatementViewModel, AdministratorViewModel administratorViewModel)
        {
            MainViewModel = mainViewModel;
            RegisterViewModel = registerViewModel;
            MonthlyStatementViewModel = monthlyStatementViewModel;
            AdministratorViewModel = administratorViewModel;

            LogOutCommand = new RelayCommand(LogOut);
            _dialogService = new DialogService();
        }

        private void LogOut()
        {
            bool confirm = _dialogService.ShowConfirm("Er du sikker på at du vil logge af?");
            if (!confirm)
                return;

            Application.Current.Shutdown();
        }
    }
}

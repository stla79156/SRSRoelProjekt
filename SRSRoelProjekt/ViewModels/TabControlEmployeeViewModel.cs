using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.ViewModels
{
    public class TabControlEmployeeViewModel : ViewModelBase
    {
        public MainViewModel MainViewModel { get; }
        public RegisterViewModel RegisterViewModel { get; }
        public MonthlyStatementViewModel MonthlyStatementViewModel { get; }
        public AdministratorViewModel AdministratorViewModel { get; }

        public TabControlEmployeeViewModel(MainViewModel mainViewModel, RegisterViewModel registerViewModel, MonthlyStatementViewModel monthlyStatementViewModel, AdministratorViewModel administratorViewModel)
        {
            MainViewModel = mainViewModel;
            RegisterViewModel = registerViewModel;
            MonthlyStatementViewModel = monthlyStatementViewModel;
            AdministratorViewModel = administratorViewModel;
        }
    }
}

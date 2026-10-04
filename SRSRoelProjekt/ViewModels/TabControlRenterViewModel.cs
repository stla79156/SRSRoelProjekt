using SRSRoelProjekt.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using SRSRoelProjekt.Core.Services;
using SRSRoelProjekt.UI.Services;
using System.Windows;

namespace SRSRoelProjekt.ViewModels
{
    public class TabControlRenterViewModel : ViewModelBase
    {
        public RenterWindowViewModel RenterWindowViewModel { get; }
        public MonthlyStatementRenterViewModel MonthlyStatementRenterViewModel { get; }
        public RelayCommand LogOutCommand { get; }
        private readonly IDialogService _dialogService;

        public TabControlRenterViewModel(RenterWindowViewModel renterWindowViewModel, MonthlyStatementRenterViewModel monthlyStatementRenterViewModel)
        {
            RenterWindowViewModel = renterWindowViewModel;
            MonthlyStatementRenterViewModel = monthlyStatementRenterViewModel;

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

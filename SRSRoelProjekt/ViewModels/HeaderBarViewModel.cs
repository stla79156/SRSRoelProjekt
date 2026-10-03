using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SRSRoelProjekt.Core.Repositories;


namespace SRSRoelProjekt.ViewModels
{
    public class HeaderBarViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly IShoppingCartRepository _shoppingCartRepo;

        public RelayCommand LogOutCommand { get; }

        public HeaderBarViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            LogOutCommand = new RelayCommand(LogOut);
        }

        private void LogOut()
        {
            bool confirm = _dialogService.ShowConfirm("Er du sikker på at du vil logge af?");
            if (!confirm)
                return;

            Application.Current.Shutdown();
            _shoppingCartRepo.ClearShoppingCart(1); // Clear the shopping cart when logging out
        }

        // Application.Current.Shutdown(); //  Application.Current.Shutdown(); skal ersttes med  new LoginWindow().Show(); når login er implementeret
    }
}

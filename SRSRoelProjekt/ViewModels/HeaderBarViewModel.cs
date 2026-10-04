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

        public HeaderBarViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        

        // Application.Current.Shutdown(); //  Application.Current.Shutdown(); skal ersttes med  new LoginWindow().Show(); når login er implementeret
    }
}

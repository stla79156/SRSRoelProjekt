using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.ViewModels
{
    public class TabControlRenterViewModel : ViewModelBase
    {
        public RenterWindowViewModel RenterWindowViewModel { get; }
        public MonthlyStatementRenterViewModel MonthlyStatementRenterViewModel { get; }

        public TabControlRenterViewModel(RenterWindowViewModel renterWindowViewModel, MonthlyStatementRenterViewModel monthlyStatementRenterViewModel)
        {
            RenterWindowViewModel = renterWindowViewModel;
            MonthlyStatementRenterViewModel = monthlyStatementRenterViewModel;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.ViewModels
{
    public class TabControlRenterViewModel : ViewModelBase
    {
        public RenterWindowViewModel RenterWindowViewModel { get; }

        public TabControlRenterViewModel(RenterWindowViewModel renterWindowViewModel)
        {
            RenterWindowViewModel = renterWindowViewModel;
        }
    }
}

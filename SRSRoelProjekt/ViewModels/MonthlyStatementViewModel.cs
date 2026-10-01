using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Repositories;
using SRSRoelProjekt.Views;
using SRSRoelProjekt.Views.UserControls;
using SRSRoelProjekt.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SRSRoelProjekt.Core.Services;
using SRSRoelProjekt.UI.Services; // DialogService-implementering
using SRSRoelProjekt.ViewModels;

namespace SRSRoelProjekt.ViewModels
{
    public class MonthlyStatementViewModel : ViewModelBase
    {

        public ObservableCollection<MonthItem> Months { get; } = new()
        {
            new MonthItem { MonthNumber = 1, MonthName = "Januar" },
            new MonthItem { MonthNumber = 2, MonthName = "Februar" },
            new MonthItem { MonthNumber = 3, MonthName = "Marts" },
            new MonthItem { MonthNumber = 4, MonthName = "April" },
            new MonthItem { MonthNumber = 5, MonthName = "Maj" },
            new MonthItem { MonthNumber = 6, MonthName = "Juni" },
            new MonthItem { MonthNumber = 7, MonthName = "Juli" },
            new MonthItem { MonthNumber = 8, MonthName = "August" },
            new MonthItem { MonthNumber = 9, MonthName = "September" },
            new MonthItem { MonthNumber = 10, MonthName = "Oktober" },
            new MonthItem { MonthNumber = 11, MonthName = "November" },
            new MonthItem { MonthNumber = 12, MonthName = "December" }
        };

        
        public MonthlyStatementViewModel()
        {
            SelectedMonth = Months[DateTime.Now.Month - 1];
        }

        //private MonthItem _selectedMonth;
        public MonthItem SelectedMonth { get; set; }
    }
}

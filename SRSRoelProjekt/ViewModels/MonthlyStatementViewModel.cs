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
        private MainViewModel _main;

        public ObservableCollection<Renter> Renters { get; } = new();
        public ObservableCollection<RenterStatement> Statements { get; } = new();
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

        public ObservableCollection<SortOption> SortOptions { get; } = new()
        {
            new SortOption { Name = "Lejernavn" },
            new SortOption { Name = "Salgsdato" },
            new SortOption { Name = "Pris" },
            new SortOption { Name = "Reolnummer" }
        };

        public MonthlyStatementViewModel()
        {
            int monthToShow = DateTime.Now.Day < 20
                ? DateTime.Now.AddMonths(-1).Month
                : DateTime.Now.Month;

            SelectedMonth = Months.First(m => m.MonthNumber == monthToShow);

            SelectedSortOption = SortOptions[0];

            Statements.Add(new RenterStatement
            {
                RenterName = "Rasmus",
                RackCount = 2,
                TotalSales = 430,
                Commission = 43,
                RackAmount = 1650,
                FinalAmount = -1263,

                Products =
{
new SoldProduct
{
ProductName = "Vase",
SoldDate = DateTime.Now,
Price = 125
},
new SoldProduct
{
ProductName = "Jakke",
SoldDate = DateTime.Now,
Price = 305
}
}
            });

        }
        //public MonthlyStatementViewModel(MainViewModel main)
        //{
        //    _main = main;

        //    int monthToShow = DateTime.Now.Day < 20
        //    ? DateTime.Now.AddMonths(-1).Month
        //    : DateTime.Now.Month;

        //    SelectedMonth = Months.First(m => m.MonthNumber == monthToShow);

        //    SelectedSortOption = SortOptions[0];
        //}

        public MonthItem SelectedMonth { get; set; }

        public SortOption SelectedSortOption { get; set; }
        
        
    }


}

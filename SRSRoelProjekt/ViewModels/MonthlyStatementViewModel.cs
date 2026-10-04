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
        private readonly IRenterRepository _renterRepository;
        private readonly IRackRepository _rackRepository;
        private readonly IProductRepository _productRepository;
        private readonly IDialogService _dialogService;
        private readonly IMonthlyPostingRepository _monthlyPostingRepository;
        private readonly Employee _loggedInEmployee = new Employee { EmployeeName = "Admin" }; // Hardcoded employee for demonstration


        public ICommand PostMonthCommand { get; }

        public ObservableCollection<RenterStatement> Statements { get; }
            = new();

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

        private MonthItem _selectedMonth;
        public MonthItem SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                OnPropertyChanged();

                LoadStatements();
            }
        }

        private SortOption _selectedSortOption;
        public SortOption SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                _selectedSortOption = value;
                OnPropertyChanged();

                LoadStatements();
            }
        }
        private void PostMonth()
        {
            if (IsMonthPosted)
            {
                _dialogService.ShowMessage(
                    $"Måneden er allerede bogført.\n{PostedInfo}");

                return;
            }
            _monthlyPostingRepository.CreatePosting(
                SelectedMonth.MonthNumber,
                DateTime.Now.Year,
                DateTime.Now,
                _loggedInEmployee.EmployeeName);

            LoadStatements();

            _dialogService.ShowMessage(
                $"Måned {SelectedMonth.MonthName} er bogført.");
        }

        public MonthlyStatementViewModel()
        {
            _renterRepository = new SqlRenterRepository();
            _rackRepository = new SQLRackRepository();
            _productRepository = new SQLProductRepository();
            _dialogService = new DialogService(); // Initialiserer DialogService
            _monthlyPostingRepository = new SqlMonthlyPostingRepository();



            PostMonthCommand = new RelayCommand(PostMonth);

            int monthToShow = DateTime.Now.Day < 20
                ? DateTime.Now.AddMonths(-1).Month
                : DateTime.Now.Month;

            SelectedMonth =
                Months.First(m => m.MonthNumber == monthToShow);

            SelectedSortOption = SortOptions[0];

            LoadStatements();
        }

        private void LoadStatements()
        {
            Statements.Clear();

            TotalMonthlySales = 0;
            TotalMonthlyCommission = 0;
            TotalMonthlyRackRent = 0;

            var posting = _monthlyPostingRepository.GetPosting(
                            SelectedMonth.MonthNumber,
                            DateTime.Now.Year);

            if (posting != null)
            {
                IsMonthPosted = true;

                PostedInfo =
                    $"Bogført {posting.PostedDate:dd-MM-yyyy HH:mm} af {posting.EmployeeName}";
            }
            else
            {
                IsMonthPosted = false;

                PostedInfo = "Ikke bogført";
            }

            var renters = _renterRepository.GetRenters();

            foreach (var renter in renters)
            {
                var renterRacks = _rackRepository
                    .GetRacks()
                    .Where(r => r.RenterId == renter.RenterId)
                    .ToList();

                var soldProducts = _productRepository
                    .GetSoldProducts(SelectedMonth.MonthNumber)
                    .Where(p => renterRacks.Any(r =>
                        r.RackNumber == p.RackNumber))
                    .ToList();

                if (!soldProducts.Any())
                    continue;

                int rackCount = renterRacks.Count;

                decimal totalSales =
                    soldProducts.Sum(p => p.Price);

                decimal commission =
                    totalSales * 0.10m;

                decimal rackAmount;

                if (rackCount == 1)
                {
                    rackAmount = 850m;
                }
                else if (rackCount <= 3)
                {
                    rackAmount = rackCount * 825m;
                }
                else
                {
                    rackAmount = rackCount * 800m;
                }

                decimal finalAmount =
                    totalSales - commission - rackAmount;

                Statements.Add(new RenterStatement
                {
                    RenterName = renter.Name,
                    RackCount = rackCount,
                    Products = new ObservableCollection<Product>(soldProducts),
                    TotalSales = totalSales,
                    Commission = commission,
                    RackAmount = rackAmount,
                    FinalAmount = finalAmount
                });

                // Company totals

                TotalMonthlySales += totalSales;
                TotalMonthlyCommission += commission;
                TotalMonthlyRackRent += rackAmount;
            }

            CompanyMonthlyIncome =
                TotalMonthlyCommission +
                TotalMonthlyRackRent;

        }



        public string CurrentDate
        {
            get => DateTime.Now.ToString("dd-MM-yyyy");
        }

        private decimal _totalMonthlySales;
        public decimal TotalMonthlySales
        {
            get => _totalMonthlySales;
            set
            {
                _totalMonthlySales = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalMonthlyCommission;
        public decimal TotalMonthlyCommission
        {
            get => _totalMonthlyCommission;
            set
            {
                _totalMonthlyCommission = value;
                OnPropertyChanged();
            }
        }

        private decimal _totalMonthlyRackRent;
        public decimal TotalMonthlyRackRent
        {
            get => _totalMonthlyRackRent;
            set
            {
                _totalMonthlyRackRent = value;
                OnPropertyChanged();
            }
        }

        private decimal _companyMonthlyIncome;
        public decimal CompanyMonthlyIncome
        {
            get => _companyMonthlyIncome;
            set
            {
                _companyMonthlyIncome = value;
                OnPropertyChanged();
            }
        }

        private bool _isMonthPosted;

        public bool IsMonthPosted
        {
            get => _isMonthPosted;
            set
            {
                _isMonthPosted = value;
                OnPropertyChanged();
            }
        }

        private string _postedInfo;

        public string PostedInfo
        {
            get => _postedInfo;
            set
            {
                _postedInfo = value;
                OnPropertyChanged();
            }
        }
    }

}
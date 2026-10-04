using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Repositories;
using SRSRoelProjekt.Core.Services;
using SRSRoelProjekt.UI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace SRSRoelProjekt.ViewModels
{
    public class MonthlyStatementRenterViewModel : ViewModelBase
    {
        private readonly Renter _loggedInRenter;
        private readonly IRackRepository _rackRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMonthlyPostingRepository _monthlyPostingRepository;

        private RenterStatement _statement;

        public RenterStatement Statement
        {
            get => _statement;
            set
            {
                _statement = value;
                OnPropertyChanged();
            }
        }

        public MonthlyStatementRenterViewModel(Renter renter)
        {
            _loggedInRenter = renter;

            _rackRepository = new SQLRackRepository();
            _productRepository = new SQLProductRepository();
            _monthlyPostingRepository = new SqlMonthlyPostingRepository();

            SelectedMonth =
                Months.First(m => m.MonthNumber == DateTime.Now.Month);

            LoadStatement();
        }

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

        private MonthItem _selectedMonth;

        public MonthItem SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                OnPropertyChanged();

                LoadStatement();
            }
        }

        private void LoadStatement()
        {
            var posting = _monthlyPostingRepository.GetPosting(
                SelectedMonth.MonthNumber,
                DateTime.Now.Year);

            if (posting != null)
            {
                IsMonthPosted = true;
                

                PostedInfo =
                    $"Bogført {posting.PostedDate:dd-MM-yyyy HH:mm}";
            }
            else
            {
                IsMonthPosted = false;

                PostedInfo = "Ikke bogført";
            }

            var renterRacks = _rackRepository
                .GetRacks()
                .Where(r => r.RenterId == _loggedInRenter.RenterId)
                .ToList();

            var soldProducts = _productRepository
                .GetSoldProducts(SelectedMonth.MonthNumber)
                .Where(p =>
                    renterRacks.Any(r =>
                        r.RackNumber == p.RackNumber))
                .ToList();

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

            Statement = new RenterStatement
            {
                RenterName = _loggedInRenter.Name,
                RackCount = rackCount,
                Products = new ObservableCollection<Product>(soldProducts),
                TotalSales = totalSales,
                Commission = commission,
                RackAmount = rackAmount,
                FinalAmount = finalAmount,
                IsPosted = IsMonthPosted
            };
        }

        public string CurrentDate
        {
            get => DateTime.Now.ToString("dd-MM-yyyy");
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

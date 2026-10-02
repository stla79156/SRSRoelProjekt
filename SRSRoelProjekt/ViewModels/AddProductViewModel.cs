using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Repositories;
using System;
using System.Windows.Input;

namespace SRSRoelProjekt.ViewModels
{
    public class AddProductViewModel : ViewModelBase
    {
        private string _productName = string.Empty;
        private string _productDescription = string.Empty;
        private decimal _productPrice = decimal.Zero;

        private readonly RenterWindowViewModel _renterWindowViewModel;
        private readonly SQLProductRepository _productRepository;

        public Action<bool?>? CloseAction { get; set; }

        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ProductNameError));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string ProductDescription
        {
            get => _productDescription;
            set
            {
                _productDescription = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ProductDescriptionError));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public decimal ProductPrice
        {
            get => _productPrice;
            set
            {
                _productPrice = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ProductPriceError));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string ProductNameError =>
            string.IsNullOrWhiteSpace(ProductName)
                ? "Produktnavnet skal udfyldes."
                : null;

        public string ProductDescriptionError =>
            string.IsNullOrWhiteSpace(ProductDescription)
                ? "Produktbeskrivelsen skal udfyldes."
                : null;

        public string ProductPriceError =>
            ProductPrice <= 0
                ? "Prisen skal være et positivt tal."
                : null;

        public RelayCommand AddProductCommand { get; }
        public RelayCommand CancelCommand { get; }

        public AddProductViewModel(
            SQLProductRepository productRepository,
            RenterWindowViewModel renterWindowViewModel)
        {
            _productRepository = productRepository;
            _renterWindowViewModel = renterWindowViewModel;

            AddProductCommand =
                new RelayCommand(AddProduct, CanAddProduct);

            CancelCommand =
                new RelayCommand(Cancel);
        }

        private bool CanAddProduct()
        {
            return string.IsNullOrWhiteSpace(ProductNameError)
                && string.IsNullOrWhiteSpace(ProductDescriptionError)
                && string.IsNullOrWhiteSpace(ProductPriceError);
        }

        private void AddProduct()
        {
            if (_renterWindowViewModel.SelectedRack == null)
                return;

            var newProduct = new Product
            {
                ProductName = ProductName,
                ProductDescription = ProductDescription,
                Price = ProductPrice,
                RackNumber = _renterWindowViewModel.SelectedRack.RackNumber
            };

            // SQL inserts product, creates ProductNumber,
            // generates EAN and updates database.
            _productRepository.AddProduct(newProduct);

            CloseAction?.Invoke(true);
        }

        private void Cancel()
        {
            CloseAction?.Invoke(false);
        }
    }
}

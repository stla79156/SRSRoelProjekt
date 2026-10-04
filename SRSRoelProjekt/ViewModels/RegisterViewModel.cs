using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Repositories;
using SRSRoelProjekt.Core.Services;
using SRSRoelProjekt.UI.Services;
using SRSRoelProjekt.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
namespace SRSRoelProjekt.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IProductRepository _productRepo;
        private readonly IDialogService _dialogService;
        private readonly IShoppingCartRepository _shoppingCartRepo;
        private readonly IPaymentRepository _paymentRepository;
        private int? _productNumber;
        private string _productName;
        private string _productDescription;
        private decimal? _price;
        private int? rackNumber;
        private int? _currentShoppingCartId;
        public string? ProductNumber
        {
            get => _productNumber?.ToString();
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _productNumber = null;
                    OnPropertyChanged();
                }
                else if (int.TryParse(value, out int parsedValue))
                {
                    _productNumber = parsedValue;
                    OnPropertyChanged();
                }
            }
        }

        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                OnPropertyChanged();
            }
        }

        public string ProductDescription
        { 
            get => _productDescription;
            set
            {
                _productDescription = value;
                OnPropertyChanged();
            }
        }

        public decimal? Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        public int? RackNumber
        {
            get => rackNumber;
            set
            {
                rackNumber = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<ShoppingCartItem> ShoppingCartItems { get; set; } 
        public Product? SelectedProduct { get; set; }
        public ShoppingCartItem? SelectedShoppingCartItem { get; set; }
        public ICommand SearchProductCommand { get; }
        public ICommand ClearSelectedProductCommand { get; }
        public ICommand AddProductToCartCommand { get; }
        public ICommand RemoveProductFromCartCommand { get; }
        public ICommand CheckoutCommand { get; }


        public RegisterViewModel()
        {
            _dialogService = new DialogService();
            Products = new ObservableCollection<Product>();
            ShoppingCartItems = new ObservableCollection<ShoppingCartItem>();
            _productRepo = new SQLProductRepository();
            _shoppingCartRepo = new SQLShoppingCartRepository();
            _paymentRepository = new SQLPaymentRepository();

            SearchProductCommand = new RelayCommand(SearchProduct);
            ClearSelectedProductCommand = new RelayCommand(ClearSelectedProduct);
            AddProductToCartCommand = new RelayCommand(AddProductToCart);
            RemoveProductFromCartCommand = new RelayCommand(RemoveProductFromCart);
            CheckoutCommand = new RelayCommand(Checkout);

        }

      

        private void SearchProduct()
        {
            if (string.IsNullOrWhiteSpace(ProductNumber))
            {
                _dialogService.ShowMessage("Indtast et produktnummer.");
                return;
            }

            SelectedProduct = _productRepo.GetProductByProductNumber(ProductNumber);

            if (SelectedProduct != null)
            {
                ProductNumber = SelectedProduct.ProductNumber.ToString();
                ProductName = SelectedProduct.ProductName;
                ProductDescription = SelectedProduct.ProductDescription;
                Price = SelectedProduct.Price;
                RackNumber = SelectedProduct.RackNumber;
                

                OnPropertyChanged(nameof(ProductNumber));
                OnPropertyChanged(nameof(ProductName));
                OnPropertyChanged(nameof(ProductDescription));
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(RackNumber));
            }
            else
            {
                _dialogService.ShowMessage("Produkt ikke fundet.");
                ClearSelectedProduct();
            }
        }

        private void AddProductToCart()
        {
            if (SelectedProduct == null)
                return;
            else if (ShoppingCartItems.Any(item => item.ProductNumber == SelectedProduct.ProductNumber))
            {
                _dialogService.ShowMessage("Produktet er allerede i indkøbskurven.");
                return;
            }

            if (_currentShoppingCartId == null)
            {
                _currentShoppingCartId = _shoppingCartRepo.CreateShoppingCart();
            }

            _shoppingCartRepo.AddProductToCart(
                _currentShoppingCartId.Value,
                SelectedProduct);

            LoadShoppingCart();
        }

        private void RemoveProductFromCart()
        {
            if (SelectedShoppingCartItem == null)
                return;

            _shoppingCartRepo.RemoveProductFromCart(_currentShoppingCartId.Value, SelectedShoppingCartItem.ProductNumber);

            ShoppingCartItems.Remove(SelectedShoppingCartItem);
            CalculateTotalPrice();
        }

        private void LoadShoppingCart()
        {
            ShoppingCartItems.Clear();
            foreach (var item in _shoppingCartRepo.GetShoppingCartItems(_currentShoppingCartId.Value))
            {
                ShoppingCartItems.Add(item);
            }
            CalculateTotalPrice();

        }

        private void ClearSelectedProduct()
        {
            ProductNumber = string.Empty;
            ProductName = string.Empty;
            ProductDescription = string.Empty;
            Price = null;
            RackNumber = null;
            SelectedProduct = null;
            OnPropertyChanged(nameof(ProductNumber));
            OnPropertyChanged(nameof(ProductName));
            OnPropertyChanged(nameof(ProductDescription));
            OnPropertyChanged(nameof(Price));
            OnPropertyChanged(nameof(RackNumber));
        }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                OnPropertyChanged();
            }
        }

        private void CalculateTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Sum(item => item.Product.Price);
        }

        private void Checkout()
        {
            if (_currentShoppingCartId == null)
            {
                _dialogService.ShowMessage("Indkøbskurven er tom.");
                return;
            }

            var paymentWindow = new PaymentMethodWindow();

            bool? result = paymentWindow.ShowDialog();

            if (result != true)
                return;

            int paymentMethodId =
                paymentWindow.ViewModel.SelectedPaymentMethodId;

            Payment payment = new Payment
            {
                PaymentDate = DateTime.Now,
                Amount = TotalPrice,
                ShoppingCartId = _currentShoppingCartId.Value,
                PaymentMethodId = paymentMethodId
            };

            _paymentRepository.CreatePayment(payment);

            foreach (var cartItem in ShoppingCartItems)
            {
                cartItem.Product.IsSold = true;
                cartItem.Product.SoldDate = DateTime.Now;
                _productRepo.UpdateProduct(cartItem.Product);
            }

            string productList = string.Join(
                Environment.NewLine,
                ShoppingCartItems.Select(i =>
                    $"{i.Product.ProductName} - {i.Product.Price} kr."));

            _dialogService.ShowMessage(
                $"Køb godkendt\n\n{productList}");

            _shoppingCartRepo.ClearShoppingCart(
                _currentShoppingCartId.Value);

            ShoppingCartItems.Clear();

            TotalPrice = 0;

            _currentShoppingCartId = null;

            ClearSelectedProduct();
        }
    }
}

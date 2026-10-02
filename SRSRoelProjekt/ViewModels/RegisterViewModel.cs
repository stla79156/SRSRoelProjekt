using System;
using SRSRoelProjekt.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Repositories;
using SRSRoelProjekt.Core.Services;
using SRSRoelProjekt.UI.Services;
namespace SRSRoelProjekt.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IProductRepository _productRepo;
        private readonly IDialogService _dialogService;
        private readonly IShoppingCartRepository _shoppingCartRepo;
        private int? _productNumber;
        private string _productName;
        private string _productDescription;
        private decimal? _price;
        private int? rackNumber;

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

        public RegisterViewModel()
        {
            _dialogService = new DialogService();
            Products = new ObservableCollection<Product>();
            ShoppingCartItems = new ObservableCollection<ShoppingCartItem>();
            _productRepo = new SQLProductRepository();
            _shoppingCartRepo = new SQLShoppingCartRepository();

            SearchProductCommand = new RelayCommand(SearchProduct);
            ClearSelectedProductCommand = new RelayCommand(ClearSelectedProduct);
            AddProductToCartCommand = new RelayCommand(AddProductToCart);
            RemoveProductFromCartCommand = new RelayCommand(RemoveProductFromCart);

        }

        /*private void LoadProducts()
        {
            var products = _productRepo.GetProducts();
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
        }*/

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

            _shoppingCartRepo.AddProductToCart(1, SelectedProduct);

            LoadShoppingCart();
        }

        private void RemoveProductFromCart()
        {
            if (SelectedShoppingCartItem == null)
                return;

            _shoppingCartRepo.RemoveProductFromCart(1, SelectedShoppingCartItem.ProductNumber);

            ShoppingCartItems.Remove(SelectedShoppingCartItem);
        }

        private void LoadShoppingCart()
        {

            foreach (var item in _shoppingCartRepo.GetShoppingCartItems(1))
            {
                ShoppingCartItems.Add(item);
            }
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
    }
}

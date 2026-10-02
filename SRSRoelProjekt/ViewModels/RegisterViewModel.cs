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
        private int _productNumber;
        private string _productName;
        private string _productDescription;
        private decimal _price;
        private int rackNumber;

        public string ProductNumber
        {
            get => _productNumber.ToString();
            set
            {
                if (int.TryParse(value, out int parsedValue))
                {
                    _productNumber = parsedValue;
                    OnPropertyChanged();
                }
                else
                {
                    // Handle invalid input (e.g., show an error message)
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

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        public int RackNumber
        { get { return rackNumber; } set { rackNumber = value; } }

        public ObservableCollection<Product> Products { get; set; }
        public Product SelectedProduct { get; set; }

        public ICommand SearchProductCommand { get; }

        public RegisterViewModel()
        {
            _dialogService = new DialogService();
            Products = new ObservableCollection<Product>();
            _productRepo = new SQLProductRepository();

            SearchProductCommand = new RelayCommand(GetProductByProductNumber);

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

        private void GetProductByProductNumber()
        {
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
                _dialogService.ShowMessage("Product not found.");
            }
        }

        private void ClearFields()
        {
            ProductNumber = string.Empty;
            ProductName = string.Empty;
            ProductDescription = string.Empty;
            Price = 0;
            RackNumber = 0;
            OnPropertyChanged(nameof(ProductNumber));
            OnPropertyChanged(nameof(ProductName));
            OnPropertyChanged(nameof(ProductDescription));
            OnPropertyChanged(nameof(Price));
            OnPropertyChanged(nameof(RackNumber));
        }
    }
}

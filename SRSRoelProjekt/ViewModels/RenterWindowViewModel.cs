using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Repositories;
using SRSRoelProjekt.Core.Services;
using SRSRoelProjekt.UI.Services;
using SRSRoelProjekt.Views.Windows;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SRSRoelProjekt.ViewModels
{
    public class RenterWindowViewModel : ViewModelBase
    {
        private readonly SQLRackRepository _rackRepository;
        private readonly SQLProductRepository _productRepository;
        private readonly Renter _loggedInRenter;
        private readonly IDialogService _dialogService;

        public ObservableCollection<Rack> Racks { get; }
            = new ObservableCollection<Rack>();

        public ObservableCollection<Product> Products { get; }
            = new ObservableCollection<Product>();
        public RelayCommand LogOutCommand { get; }
        private Rack _selectedRack;

        public Rack SelectedRack
        {
            get => _selectedRack;
            set
            {
                _selectedRack = value;
                OnPropertyChanged();
                LoadProducts();
            }
        }

        private Product _selectedProduct;

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;

                OnPropertyChanged();

                CommandManager.InvalidateRequerySuggested();
            }
        }

        public RelayCommand AddProductCommand { get; }
        public RelayCommand RemoveProductCommand { get; }


        public RenterWindowViewModel(Renter renter)
        {
            _loggedInRenter = renter;
            _dialogService = new DialogService();

            _rackRepository = new SQLRackRepository();
            _productRepository = new SQLProductRepository();

            
            var racks = _rackRepository
                .GetRacks()
                .Where(r => r.RenterId == renter.RenterId);

            foreach (var rack in racks)
            {
                Racks.Add(rack);
            }
           
            AddProductCommand =
                new RelayCommand(OpenAddProductWindow, CanAddProduct);
            RemoveProductCommand =
                new RelayCommand(RemoveProduct, CanRemoveProduct);
            LogOutCommand = new RelayCommand(LogOut);

        }
        private void LogOut()
        {
            bool confirm = _dialogService.ShowConfirm("Er du sikker på at du vil logge af?");
            if (!confirm)
                return;

            Application.Current.Shutdown();
        }

        private bool CanAddProduct()
        {
            return SelectedRack != null;
                
        }

        private void RemoveProduct()
        {
            if (SelectedProduct == null)
                return;

            bool confirm = _dialogService.ShowConfirm(
                $"Er du sikker på at du vil fjerne følgende produkt?\n\n" +
                $"Navn: {SelectedProduct.ProductName}\n" +
                $"Beskrivelse: {SelectedProduct.ProductDescription}\n" +
                $"Pris: {SelectedProduct.Price:C}"
            );

            if (!confirm)
                return;

            _productRepository.RemoveProduct(SelectedProduct);

            Products.Remove(SelectedProduct);

            SelectedProduct = null;

            OnPropertyChanged(nameof(SelectedProduct));
        }
        private bool CanRemoveProduct()
        {
            return SelectedProduct != null;
        }

        private void OpenAddProductWindow()
        {
            var window = new AddProductWindow();

            var vm = new AddProductViewModel(
                _productRepository,
                this);

            vm.CloseAction = result =>
            {
                window.DialogResult = result;
                window.Close();

                // reload products after adding one
                LoadProducts();
            };

            window.DataContext = vm;

            window.ShowDialog();
        }

        private void LoadProducts()
        {
            Products.Clear();

            if (SelectedRack == null)
                return;

            foreach (var product in
                     _productRepository.GetProductsByRack(
                         SelectedRack.RackNumber))
            {
                Products.Add(product);
            }
        }

        public string WelcomeText =>
    _loggedInRenter == null
        ? "Velkommen"
        : $"Velkommen {_loggedInRenter.Name}";
    }
}
using SRSRoelProjekt.Commands;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Services;
using SRSRoelProjekt.Core.Repositories;
using SRSRoelProjekt.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SRSRoelProjekt.ViewModels
{
    public class ProductControlViewModel : ViewModelBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IDialogService _dialogService;

        public ICommand AddProductCommand { get; }
        public ICommand RemoveProductCommand { get; }

        public ProductControlViewModel(IProductRepository productRepository, IDialogService dialogService)
        {
            _productRepository = productRepository;
            _dialogService = dialogService;

            AddProductCommand = new RelayCommand(OpenAddProductWindow);
            RemoveProductCommand = new RelayCommand(RemoveProduct);
        }

        private void OpenAddProductWindow()
        {
            //var addProductWindow = new AddProductWindow();
            //addProductWindow.ShowDialog();
        }

        private void RemoveProduct()
        {
            // Implementation for removing a product
        }

    }
}

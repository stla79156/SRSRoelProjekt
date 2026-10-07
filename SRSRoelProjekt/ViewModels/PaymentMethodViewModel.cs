using SRSRoelProjekt.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SRSRoelProjekt.Core.Models;

namespace SRSRoelProjekt.ViewModels
{
    public class PaymentMethodViewModel : ViewModelBase
    {
        private readonly Window _window;

        public PaymentMethod SelectedPaymentMethod { get; set; }

        public ICommand MobilePayCommand { get; }
        public ICommand CashCommand { get; }

        public PaymentMethodViewModel(Window window)
        {
            _window = window;

            MobilePayCommand = new RelayCommand(SelectMobilePay);
            CashCommand = new RelayCommand(SelectCash);
        }

        private void SelectMobilePay()
        {
            SelectedPaymentMethod = new PaymentMethod { PaymentMethodId = 1 };
            _window.DialogResult = true;
            _window.Close();
        }

        private void SelectCash()
        {
            SelectedPaymentMethod = new PaymentMethod { PaymentMethodId = 2 };
            _window.DialogResult = true;
            _window.Close();
        }
    }
}

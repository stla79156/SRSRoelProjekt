using SRSRoelProjekt.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SRSRoelProjekt.ViewModels
{
    public class PaymentMethodViewModel : ViewModelBase
    {
        private readonly Window _window;

        public int SelectedPaymentMethodId { get; private set; }

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
            SelectedPaymentMethodId = 1;
            _window.DialogResult = true;
            _window.Close();
        }

        private void SelectCash()
        {
            SelectedPaymentMethodId = 2;
            _window.DialogResult = true;
            _window.Close();
        }
    }
}

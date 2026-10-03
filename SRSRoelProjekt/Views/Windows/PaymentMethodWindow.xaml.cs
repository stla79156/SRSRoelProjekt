using SRSRoelProjekt.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SRSRoelProjekt.Views.Windows
{
    /// <summary>
    /// Interaction logic for PaymentMethodWindow.xaml
    /// </summary>
    public partial class PaymentMethodWindow : Window
    {
        public PaymentMethodViewModel ViewModel { get; }

        public PaymentMethodWindow()
        {
            InitializeComponent();

            ViewModel = new PaymentMethodViewModel(this);
            DataContext = ViewModel;
        }
    }
}

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
using SRSRoelProjekt.ViewModels;

namespace SRSRoelProjekt.Views.Windows
{
    /// <summary>
    /// Interaction logic for MonlthyStatementWindow.xaml
    /// </summary>
    public partial class MonthlyStatementWindow : Window
    {
        public MonthlyStatementWindow()
        {
            InitializeComponent();
            DataContext = new MonthlyStatementViewModel();
        }

        
    }
}

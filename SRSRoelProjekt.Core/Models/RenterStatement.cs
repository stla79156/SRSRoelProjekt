
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Repositories;
using SRSRoelProjekt.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace SRSRoelProjekt.Core.Models
{
    public class RenterStatement
    {
        public bool IsPosted { get; set; }
        public string RenterName { get; set; }
        public int RackCount { get; set; }
        public ObservableCollection<Product> Products { get; set; }
            = new();
        public decimal TotalSales { get; set; }
        public decimal Commission { get; set; }
        public decimal RackAmount { get; set; }
        public decimal FinalAmount { get; set; }
    }
}

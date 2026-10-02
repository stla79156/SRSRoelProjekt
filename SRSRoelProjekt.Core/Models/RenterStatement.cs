using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SRSRoelProjekt.Core.Models
{
    public class RenterStatement
    {
        public string RenterName { get; set; }

        public int RackCount { get; set; }

        public decimal TotalSales { get; set; }

        public decimal Commission { get; set; }

        public decimal RackAmount { get; set; }

        public decimal FinalAmount { get; set; }

        public ObservableCollection<SoldProduct> Products
        {
            get;
            set;
        } = new();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Models
{
    public class Product
    {
        public int ProductNumber { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public string EAN13Number { get; set; }
        public int RackNumber { get; set; }
        public bool ProductStatus { get; set; } // true = Not Sold, false = Sold
    }







}

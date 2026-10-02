using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartId { get; set; }
        public int ProductNumber { get; set; }

        public Product Product { get; set; }
    }
}

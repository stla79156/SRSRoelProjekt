using System;
using SRSRoelProjekt.Core.Models;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Repositories
{
    public interface IShoppingCartRepository
    {
        List<Product> GetShoppingCartItems();
        void AddProductToCart(Product product);
        void RemoveProductFromCart(Product product);
    }
}

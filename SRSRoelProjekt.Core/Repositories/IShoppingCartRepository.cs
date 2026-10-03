using System;
using SRSRoelProjekt.Core.Models;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Repositories
{
    public interface IShoppingCartRepository
    {
        List<ShoppingCartItem> GetShoppingCartItems(int shoppingCartId);
        void AddProductToCart(int shoppingCartId, Product product);
        void RemoveProductFromCart(int shoppingCartId, int productNumber);
        void ClearShoppingCart(int shoppingCartId);
        int CreateShoppingCart();
    }
}

using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Repositories
{
    public interface IProductRepository
    {
        List<Product> GetProductsByRack(int rackNumber);

        List<Product> GetProducts();

        //void SaveProducts(List<Product> products);

        void AddProduct(Product product);
        void RemoveProduct(Product product);
        void UpdateProduct(Product product);
        Product GetProductByProductNumber(string productNumber);

    }
}

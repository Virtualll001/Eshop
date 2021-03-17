using Eshop.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Business.Interfaces
{
    public interface IProductManager
    {
        Product FindProductById(int id);

        Product FindProductByUrl(string url);

        void SaveProduct(Product product);

        //Hlavičku metod SaveProductImages();RemoveProductImage(); nezapomenout přidat [9. Managery].
    }
}

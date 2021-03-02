using Eshop.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Data.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Product FindByUrl(string url);
    }
}

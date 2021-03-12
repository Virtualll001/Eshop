using Eshop.Business.Interfaces;
using Eshop.Data.Interfaces;
using Eshop.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Business.Managers
{
    public class ProductManager : IProductManager
    {
        private IProductRepository productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public Product FindProductById(int id)
        {
            return productRepository.FindById(id);
        }
        public Product FindProductByUrl(string url)
        {
            return productRepository.FindByUrl(url);
        }
        public void SaveProduct(Product product)
        {
            productRepository.Insert(product);
        }
    }
}

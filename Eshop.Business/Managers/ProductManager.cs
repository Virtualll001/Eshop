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
            var oldProduct = productRepository.FindById(product.ProductId);

            if (product.ProductId != 0)
                product.ProductId = 0;

            productRepository.Insert(product);

            if (oldProduct != null)
                CleanProduct(oldProduct);
        }

        public void CleanProduct(Product oldProduct)
        {
            try
            {
                productRepository.Delete(oldProduct.ProductId);
            }
            catch (Exception)
            {
                oldProduct.CategoryProducts.Clear(); // odstraníme produkt z kategorií
                oldProduct.Hidden = true;            // a skryjeme jej
                productRepository.Update(oldProduct);
            }
        }

        public bool[] FindAssignedCategoriesToProduct(List<Category> availableCategories, List<CategoryProduct> assignedCategories, bool[] postedCategories)
        {
            for (int a = 0; a < availableCategories.Count; a++)
            {
                for (int p = 0; p < assignedCategories.Count; p++)
                {
                    if (availableCategories[a].CategoryId == assignedCategories[p].CategoryId)
                        postedCategories.SetValue(true, a);

                    break;
                }
            }

            return postedCategories;
        }
    }    
}

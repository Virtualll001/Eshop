using Eshop.Business.Interfaces;
using Eshop.Data.Interfaces;
using Eshop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eshop.Business.Managers
{
    public class CategoryManager : ICategoryManager
    {
        private ICategoryRepository categoryRepository;
        private IProductRepository productRepository;

        public CategoryManager(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
        }

        public List<Category> GetCategoriesWithoutChildCategories()
        {
            return categoryRepository.GetCategoriesWithoutChildCategories();
        }

        public void UpdateProductCategories(int productId, int[] categories)
        {
            var product = productRepository.FindById(productId)
                ?? throw new ArgumentNullException($"Produkt {productId} nebyl nalezen");

            var currentCategories = product.CategoryProducts.Select(cp => cp.CategoryId);

            var removeCategories = currentCategories.Except(currentCategories).ToList();

            var addCategories = categories.Except(currentCategories).ToList();

            foreach (var categoryId in removeCategories)
            {
                var toRemove = product.CategoryProducts
                    .Where(cp => cp.CategoryId == categoryId)
                    .SingleOrDefault();

                product.CategoryProducts.Remove(toRemove);
            }

            foreach (var categoryId in addCategories)
            {
                var toAdd = new CategoryProduct()
                {
                    CategoryId = categoryId,
                    ProductId = product.ProductId
                };
                product.CategoryProducts.Add(toAdd);
            }

            productRepository.Update(product);
        }


    }
}

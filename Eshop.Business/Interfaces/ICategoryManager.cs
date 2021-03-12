using Eshop.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Business.Interfaces
{
    public interface ICategoryManager
    {
        List<Category> GetCategoriesWithoutChildCategories();

        void UpdateProductCategories(int productId, int[] categories);
    }
}

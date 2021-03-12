using Eshop.Data.Interfaces;
using Eshop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eshop.Data.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
        public List<Category> GetCategoriesWithoutChildCategories()
        {
            return dbSet.Where(x => x.ChildCategories.Count == 0 && x.Hidden == false).ToList();
        }
    }
}

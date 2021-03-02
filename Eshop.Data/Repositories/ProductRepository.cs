using Eshop.Data.Interfaces;
using Eshop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eshop.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        { }

        public Product FindByUrl(string url)
        {
            return dbSet.SingleOrDefault(x => x.Url == url && !x.Hidden);
        }
    }
}

using Eshop.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eshop.Data.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext eshopContext;
        protected DbSet<TEntity> dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            eshopContext = context;
            dbSet = eshopContext.Set<TEntity>();
        }

        public List<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        public TEntity FindById(int id)
        {
            return dbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            dbSet.Add(entity);
            eshopContext.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            if (dbSet.Contains(entity))
                dbSet.Update(entity);

            else
                dbSet.Add(entity);

            eshopContext.SaveChanges();
        }

        public void Delete(int id)
        {
            TEntity entity = dbSet.Find(id);
            try
            {
                dbSet.Remove(entity);
                eshopContext.SaveChanges();
            }
            catch (Exception)
            {
                eshopContext.Entry(entity).State = EntityState.Unchanged;
                throw;
            }
        }
    }
}

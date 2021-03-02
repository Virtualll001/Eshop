using System;
using System.Collections.Generic;
using System.Text;

namespace Eshop.Data.Interfaces
{
    public interface IRepository<TEntity>
    {
        List<TEntity> GetAll();

        TEntity FindById(int id);

        void Insert(TEntity entity);

        void Update(TEntity entity);

        void Delete(int id);
    }
}

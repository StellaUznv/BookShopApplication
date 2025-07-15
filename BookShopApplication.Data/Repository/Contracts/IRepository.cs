using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Data.Repository.Contracts
{
    public interface IRepository<TEntity, TKey>
    {
        TEntity? GetById(TKey id);

        TEntity? FirstOrDefault(Func<TEntity, bool> predicate);
        IEnumerable<TEntity> GetAll();

        IQueryable<TEntity> GetAllAttached();

        void Add(TEntity item);

        void AddRange(TEntity[] items);

        bool Delete(TEntity entity);
        bool Update(TEntity item);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApplication.Data.Repository.Contracts
{
    public interface IAsyncRepository<TEntity, TKey>
    {
        Task<TEntity?> GetByIdAsync(TKey id);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity item);
        Task AddRangeAsync(TEntity[] items);
        Task<bool> DeleteAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity item);
        Task SaveChangesAsync();
    }
}

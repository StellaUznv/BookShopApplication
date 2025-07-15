using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BookShopApplication.Data.Repository
{
    public abstract class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey> 
        , IRepositoryAsync<TEntity,TKey>
    where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> dbSet;

        protected BaseRepository(ApplicationDbContext context)
        {
            this._context = context;
            this.dbSet = this._context.Set<TEntity>();
        }

        public TEntity? GetById(TKey id)
        {
            return this.dbSet.Find(id);
        }

        public TEntity? FirstOrDefault(Func<TEntity, bool> predicate)
        {
            return this.dbSet.FirstOrDefault(predicate);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this.dbSet.ToList();
        }

        public IQueryable<TEntity> GetAllAttached()
        {
            return this.dbSet.AsQueryable();
        }

        public void Add(TEntity item)
        {
            this.dbSet.Add(item);
            this._context.SaveChanges();
        }

        public void AddRange(TEntity[] items)
        {
            this.dbSet.AddRange(items);
            this._context.SaveChanges();
        }

        public bool Delete(TEntity entity)
        {
            this.dbSet.Remove(entity);
            return _context.SaveChanges() > 0;
        }

        public bool Update(TEntity item)
        {
            this.dbSet.Update(item);
            return _context.SaveChanges() > 0;
        }

        public Task<TEntity> GetByIdAsync(TKey id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(TEntity[] items)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}

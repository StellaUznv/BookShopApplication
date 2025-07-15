using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BookShopApplication.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using static BookShopApplication.GCommon.ExceptionMessages.ExceptionMessages.Data;

namespace BookShopApplication.Data.Repository
{
    public abstract class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey> 
        , IAsyncRepository<TEntity,TKey>
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

        public bool SoftDelete(TEntity item)
        {
            this.PerformSoftDeleteOfEntity(item);
            return this.Update(item);
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await this.dbSet.FindAsync(id);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await this.dbSet.ToListAsync();
        }

        public async Task AddAsync(TEntity item)
        {
             await this.dbSet.AddAsync(item);
             await this._context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(TEntity[] items)
        {
            await this.dbSet.AddRangeAsync(items);
            await this._context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            this.dbSet.Remove(entity);
            return await this._context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(TEntity item)
        {
            this.dbSet.Update(item);
            return await this._context.SaveChangesAsync() > 0;
        }

        public async Task SaveChangesAsync()
        {
           await this._context.SaveChangesAsync();
        }

        public async Task<bool> SoftDeleteAsync(TEntity entity)
        {
            this.PerformSoftDeleteOfEntity(entity);
            return await this.UpdateAsync(entity);
        }

        private void PerformSoftDeleteOfEntity(TEntity entity)
        {
            PropertyInfo? isDeletedProperty =
                this.GetIsDeletedProperty(entity);
            if (isDeletedProperty == null)
            {
                throw new InvalidOperationException(SoftDeleteOnNonSoftDeletableEntity);
            }

            isDeletedProperty.SetValue(entity, true);
        }

        private PropertyInfo? GetIsDeletedProperty(TEntity entity)
        {
            return typeof(TEntity)
                .GetProperties()
                .FirstOrDefault(pi => pi.PropertyType == typeof(bool) &&
                                      pi.Name == "IsDeleted");
        }
    }
}

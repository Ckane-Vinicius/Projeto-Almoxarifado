using System.Collections.Generic;
using System.Linq.Expressions;
using ApiAlmoxarifado.Data.Context;
using ApiAlmoxarifado.Data.Repository_Interfaces;
using ApiAlmoxarifado.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiAlmoxarifado.Data.Repository
{
    public class RepositoryBase<TEntity> : IDisposable, IRepositoryBase<TEntity> where TEntity : Entity
    {
        protected readonly ApiContext context;
        protected DbSet<TEntity> DbSet;

        public RepositoryBase(ApiContext dbContext)
        {
            context = dbContext;
            DbSet = context.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            DbSet.Add(entity);
            SaveChanges();
        }

        public void AddRange(List<TEntity> entity)
        {
            DbSet.AddRange(entity);
            SaveChanges();
        }

        public void AddOrUpdate(TEntity entity)
        {
            if (entity == null)
                return;

            if (entity.Id == 0)
                Add(entity);
            else
                Update(entity);
        }

        public TEntity GetById(int id)
        {
            return DbSet.Find(id);
        }

        public async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await DbSet.FindAsync(id, cancellationToken);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).FirstOrDefault();
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }
        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
            SaveChanges();
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
            SaveChanges();
        }

        private void SaveChanges()
        {
            context.SaveChanges();
        }

        public async Task AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
        {
            await DbSet.AddRangeAsync(entities, ct);
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (context != null)
                context.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}

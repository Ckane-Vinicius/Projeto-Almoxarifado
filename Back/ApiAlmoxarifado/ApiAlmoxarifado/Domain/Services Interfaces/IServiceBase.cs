using System.Linq.Expressions;

namespace ApiAlmoxarifado.Domain.Services_Interfaces
{
    public interface IServiceBase<TEntity>
    {
        TEntity GetById(int id);
        Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);
        bool Exists(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(List<TEntity> entity, CancellationToken ct = default);
        void AddRange(List<TEntity> entity);
        void Update(TEntity entity);
        void AddOrUpdate(TEntity entity);
        void Remove(TEntity entity);
    }
}

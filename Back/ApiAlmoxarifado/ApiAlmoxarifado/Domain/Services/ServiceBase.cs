using System.Linq.Expressions;
using ApiAlmoxarifado.Data.Repository_Interfaces;
using ApiAlmoxarifado.Domain.Services_Interfaces;

namespace ApiAlmoxarifado.Domain.Services
{
    public class ServiceBase<TEntity> : IDisposable, IServiceBase<TEntity> where TEntity : class
    {
        private readonly IRepositoryBase<TEntity> _repositoryBase;

        public ServiceBase(IRepositoryBase<TEntity> repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }

        public virtual TEntity GetById(int id)
        {
            return _repositoryBase.GetById(id);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _repositoryBase.Get(predicate);
        }

        public async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _repositoryBase.GetByIdAsync(id, cancellationToken);
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            return _repositoryBase.GetList(predicate);
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return _repositoryBase.Exists(predicate);
        }

        public virtual void Add(TEntity entity)
        {
            _repositoryBase.Add(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _repositoryBase.AddAsync(entity);
        }

        public async Task AddRangeAsync(List<TEntity> entity, CancellationToken ct = default)
        {
            await _repositoryBase.AddRangeAsync(entity, ct);
        }

        public virtual void AddRange(List<TEntity> entity)
        {
            _repositoryBase.AddRange(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _repositoryBase.Update(entity);
        }

        public virtual void AddOrUpdate(TEntity entity)
        {
            _repositoryBase.AddOrUpdate(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            _repositoryBase.Remove(entity);
        }

        public void Dispose()
        {
            _repositoryBase.Dispose();
        }
    }
}

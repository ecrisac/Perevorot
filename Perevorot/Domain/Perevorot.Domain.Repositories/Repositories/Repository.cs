using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Perevorot.Domain.Core.Infrastructure;
using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.Repositories.Repositories
{
    public class Repository : IRepository
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        protected Repository(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return _unitOfWorkFactory.Create();
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : PerevorotEntity
        {
            return GetDbContext().Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> includes)
            where TEntity : PerevorotEntity
        {
            Debug.Assert(includes != null);
            if (includes == null) throw new ArgumentNullException("includes");

            IQueryable<TEntity> entities = GetDbContext().Set<TEntity>();
            return entities.Where(includes);
        }

        public IQueryable<TEntity> GetAll<TEntity>(IEnumerable<long> entityIds) where TEntity : PerevorotEntity
        {
            return GetDbContext().Set<TEntity>().Where(x => entityIds.Contains(x.Id));
        }

        public TEntity GetById<TEntity>(long id) where TEntity : PerevorotEntity
        {
            return GetDbContext().Set<TEntity>().Find(id);
        }

        public TEntity SaveOrUpdate<TEntity>(TEntity entity) where TEntity : PerevorotEntity
        {
            if (entity.Id == 0) return GetDbContext().Set<TEntity>().Add(entity);
            return GetById<TEntity>(entity.Id) == null ? GetDbContext().Set<TEntity>().Add(entity) : entity;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : PerevorotEntity
        {
            GetDbContext().Set<TEntity>().Remove(entity);
        }

        public TEntity Attach<TEntity>(TEntity entity) where TEntity : PerevorotEntity
        {
            return GetDbContext().Set<TEntity>().Attach(entity);
        }

        public TEntity GetDetached<TEntity>(long id) where TEntity : PerevorotEntity
        {
            return GetDbContext().Set<TEntity>().AsNoTracking().SingleOrDefault(x => x.Id == id);
        }

        public void SetModified<TEntity>(TEntity entity) where TEntity : PerevorotEntity
        {
            GetDbContext().Entry(entity).State = EntityState.Modified;
        }

        public void ResetReference<TEntity, TReference>(TEntity entity, Expression<Func<TEntity, TReference>> nav)
            where TEntity : PerevorotEntity
            where TReference : PerevorotEntity
        {
            GetDbContext().Entry(entity).Reference(nav).CurrentValue = null;
        }

        private PerevorotEntities GetDbContext()
        {
            PerevorotEntities context = _unitOfWorkFactory.GetCurrentContext();
            if (context == null) throw new ArgumentNullException("context", "Db context cannot be null");
            return context;
        }
    }
}
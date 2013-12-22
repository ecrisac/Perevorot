using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Perevorot.Domain.Models.DomainEntities;

namespace Perevorot.Domain.Core.Infrastructure
{
    public interface IRepository
    {
        IUnitOfWork CreateUnitOfWork();

        IQueryable<TEntity> GetAll<TEntity>() where TEntity : PerevorotEntity;

        IQueryable<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> includes) where TEntity : PerevorotEntity;

        IQueryable<TEntity> GetAll<TEntity>(IEnumerable<long> entityIds) where TEntity : PerevorotEntity;

        TEntity GetById<TEntity>(long id) where TEntity : PerevorotEntity;

        TEntity SaveOrUpdate<TEntity>(TEntity entity) where TEntity : PerevorotEntity;

        void Delete<TEntity>(TEntity entity) where TEntity : PerevorotEntity;

        TEntity Attach<TEntity>(TEntity entity) where TEntity : PerevorotEntity;

        TEntity GetDetached<TEntity>(long id) where TEntity : PerevorotEntity;

        void SetModified<TEntity>(TEntity entity) where TEntity : PerevorotEntity;

        void ResetReference<TEntity, TReference>(TEntity entity, Expression<Func<TEntity, TReference>> nav)
            where TEntity : PerevorotEntity
            where TReference : PerevorotEntity;
       
    }
}
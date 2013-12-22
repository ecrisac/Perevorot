using System;

namespace Perevorot.Domain.Core.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        [ThreadStatic]
        private static PerevorotEntities context;

        private readonly bool owner;

        public UnitOfWork()
        {
            if (context == null)
            {
                owner = true;
                context = new PerevorotEntities();
            }
        }

        #region IUnitOfWork Members

        public void Dispose()
        {
            if (owner)
            {
                context.Dispose();
                context = null;
            }
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        #endregion

        public static PerevorotEntities GetContext()
        {
            return context;
        }
    }
}
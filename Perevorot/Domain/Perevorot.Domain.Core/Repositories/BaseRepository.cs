using System;
using Perevorot.Domain.Core.Infrastructure;

namespace Perevorot.Domain.Core.Repositories
{
    public abstract class Repository : IDisposable
    {
        private readonly PerevorotEntities _session;

        protected Repository()
        {
            _session = new PerevorotEntities();
        }

        protected PerevorotEntities Session
        {
            get { return _session; }
        }

        public void Dispose()
        {
            Session.Dispose();
        }
    }
}
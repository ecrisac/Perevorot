using System;

namespace Perevorot.Domain.Core.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
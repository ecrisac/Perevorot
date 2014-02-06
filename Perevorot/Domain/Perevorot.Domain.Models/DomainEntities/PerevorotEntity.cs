using System;

namespace Perevorot.Domain.Models.DomainEntities
{
    public abstract class PerevorotEntity
    {
        protected PerevorotEntity()
        {
            Created = DateTime.UtcNow;
        }

        public long Id { get; set; }

        public DateTimeOffset Created { get; set; }
    }
}
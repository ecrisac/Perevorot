using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perevorot.Domain.Models.DomainEntities
{
    public abstract class PerevorotEntity
    {
        protected PerevorotEntity()
        {
            Created = DateTime.UtcNow;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime Created { get; set; }
    }
}
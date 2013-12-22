using System;

namespace Perevorot.Domain.Models.DomainEntities
{
    public class Customer : PerevorotEntity
    {
        public Customer(string name)
        {
            Name = name;
        }

        [Obsolete("EF Ctor only!")]
        public Customer()
        {
        }

        public String Name { get; private set; }
    }
}
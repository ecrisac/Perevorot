using System;

namespace Perevorot.Domain.Models.DomainEntities
{
    public class Customer
    {
        public Customer(string name)
        {
            Name = name;
        }

        [Obsolete("EF Ctor only!")]
        public Customer()
        {
        }

        public long Id { get; private set; }

        public String Name { get; private set; }
    }
}
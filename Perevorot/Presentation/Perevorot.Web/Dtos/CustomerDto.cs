using System;
using System.Runtime.Serialization;

namespace Perevorot.Web.Dtos
{
    [DataContract]
    [Serializable]
    public class CustomerDto
    {
        [DataMember]
        public long Id;

        [DataMember]
        public string CustomerName;

        [DataMember]        
        public DateTime CreationDate;
    }
}
using Perevorot.Domain.Models.Enums;

namespace Perevorot.Domain.Models.DomainEntities
{
    public class AccessRight : PerevorotEntity
    {
        public AccessRight(string resourseId, AccessRightType accessRightType)
        {
            ResourseId = resourseId;
            AccessRightType = accessRightType;
        }

        public string ResourseId { get; private set; }
        public AccessRightType AccessRightType { get; private set; }

        // Foreign key
       // public UserGroup UserGroup { get; set; }
    }
}
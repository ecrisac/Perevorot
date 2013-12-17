using Perevorot.Domain.Models.Enums;

namespace Perevorot.Domain.Models.DomainEntities
{
    public class AccessRight
    {
        public long Id { get; private set; }

        public string ResourseId { get; private set; }
        public AccessRightType AccessRightType { get; private set; }

        // Foreign key
        public UserGroup UserGroup { get; set; }

        public AccessRight(string resourseId, AccessRightType accessRightType)
        {
            ResourseId = resourseId;
            AccessRightType = accessRightType;
        }
    }
}
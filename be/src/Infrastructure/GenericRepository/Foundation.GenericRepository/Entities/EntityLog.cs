using System;

namespace Foundation.GenericRepository.Entities
{
    public class EntityLog
    {
        public Guid? EntityId { get; private set; }
        public string EntityType { get; private set; }
        public string Content { get; private set; }
        public string OldValue { get; private set; }
        public string NewValue { get; private set; }
        public string Source { get; set; }
        public string IpAddress { get; set; }
        public string Username { get; set; }
        public Guid? OrganisationId { get; set; }
    }
}

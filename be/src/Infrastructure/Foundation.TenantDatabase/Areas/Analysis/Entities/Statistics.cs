using Foundation.GenericRepository.Entities;
using System;

namespace Foundation.TenantDatabase.Areas.Analysis.Entities
{
    public class Statistics : Entity
    {
        public Guid EntityTypeId { get; set; }
        public Guid EntityId { get; set; }
        public Guid ActionTypeId { get; set; }
        public string Value { get; set; }
    }
}

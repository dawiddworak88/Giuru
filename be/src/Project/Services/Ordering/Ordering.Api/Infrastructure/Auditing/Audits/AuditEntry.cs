using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Infrastructure.Auditing.Audits
{
    public class AuditEntry : Entity
    {
        [Required]
        public string EntityType { get; set; }

        [Required]
        public Guid EntityId { get; set; }

        [Required]
        public string OldValue { get; set; }

        [Required]
        public string NewValue { get; set; }

        [Required]
        public string Source { get; set; }

        public string IpAddress { get; set; }
    }
}

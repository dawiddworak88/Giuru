using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventLogging.Api.Infrastructure.Events.Entities
{
    public class EventLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid? EventId { get; set; }

        public string EventState { get; set; }

        public string EntityType { get; set; }

        public Guid? EntityId { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string Content { get; set; }

        public Guid? OrganisationId { get; set; }

        public string Username { get; set; }

        public string Source { get; set; }

        public string IpAddress { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}

using Foundation.GenericRepository;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foundation.TenantDatabase.Areas.Clients.Entities
{
    public class Client : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Host { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string LastModifiedBy { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}

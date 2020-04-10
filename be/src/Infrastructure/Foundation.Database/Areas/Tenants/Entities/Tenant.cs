using Foundation.GenericRepository;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foundation.Database.Areas.Tenants.Entities
{
    public class Tenant : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Host { get; set; }

        [Required]
        public string DatabaseConnectionString { get; set; }

        [Required]
        public string StorageConnectionString { get; set; }

        [Required]
        public string QueueConnectionString { get; set; }

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

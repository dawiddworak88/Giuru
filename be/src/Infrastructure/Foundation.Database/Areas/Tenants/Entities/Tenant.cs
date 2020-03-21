using Foundation.Database.Areas.Accounts.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foundation.Database.Areas.Tenants.Entities
{
    public class Tenant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ApplicationUser LastModifiedBy { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}

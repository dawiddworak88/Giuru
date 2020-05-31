using Foundation.GenericRepository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.TenantDatabase.Areas.Taxonomies.Entities
{
    public class Taxonomy : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        public Guid? ParentId { get; set; }
    }
}

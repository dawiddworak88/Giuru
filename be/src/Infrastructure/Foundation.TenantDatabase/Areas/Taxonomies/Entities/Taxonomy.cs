using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foundation.TenantDatabase.Areas.Taxonomies.Entities
{
    public class Taxonomy : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        [ForeignKey("ParentId")]
        public virtual Taxonomy Parent { get; set; }
    }
}

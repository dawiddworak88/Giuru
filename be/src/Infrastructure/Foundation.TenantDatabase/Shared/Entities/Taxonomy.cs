using Foundation.GenericRepository.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foundation.TenantDatabase.Shared.Entities
{
    public class Taxonomy : Entity
    {
        public virtual Item TaxonomyItem { get; set; }

        public virtual Item ValueItem { get; set; }

        [Required]
        public int Order { get; set; }

        [ForeignKey("ParentId")]
        public virtual Taxonomy Parent { get; set; }
    }
}

using Foundation.Extensions.Models;
using System;

namespace Foundation.Taxonomy.Models
{
    public class CreateTaxonomyModel : BaseServiceModel
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
    }
}

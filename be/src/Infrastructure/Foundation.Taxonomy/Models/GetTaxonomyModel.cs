using Foundation.Extensions.Models;
using System;

namespace Foundation.Taxonomy.Models
{
    public class GetTaxonomyModel : BaseServiceModel
    {
        public string Name { get; set; }
        public Guid? RootId { get; set; }
    }
}

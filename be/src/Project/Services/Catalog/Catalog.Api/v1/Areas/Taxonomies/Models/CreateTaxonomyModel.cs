using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.v1.Areas.Taxonomies.Models
{
    public class CreateTaxonomyModel : BaseServiceModel
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
    }
}

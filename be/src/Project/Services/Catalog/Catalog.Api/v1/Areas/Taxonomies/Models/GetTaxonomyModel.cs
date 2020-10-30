using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.v1.Areas.Taxonomies.Models
{
    public class GetTaxonomyModel : BaseServiceModel
    {
        public string Name { get; set; }
        public Guid? RootId { get; set; }
    }
}

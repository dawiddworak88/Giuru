using Catalog.Api.Infrastructure.Taxonomies.Entities;
using Foundation.Extensions.Models;

namespace Catalog.Api.v1.Areas.Taxonomies.ResultModels
{
    public class TaxonomyResultModel : BaseServiceResultModel
    {
        public Taxonomy Taxonomy { get; set; }
    }
}

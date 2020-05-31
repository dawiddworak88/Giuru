using Foundation.Extensions.Models;

namespace Foundation.Taxonomy.ResultModels
{
    public class TaxonomyResultModel : BaseServiceResultModel
    {
        public TenantDatabase.Areas.Taxonomies.Entities.Taxonomy Taxonomy { get; set; }
    }
}

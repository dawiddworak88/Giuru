using Foundation.ApiExtensions.Models.Request;
using System;

namespace Catalog.Api.v1.Areas.Taxonomies.RequestModels
{
    public class TaxonomyRequestModel : BaseRequestModel
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
    }
}

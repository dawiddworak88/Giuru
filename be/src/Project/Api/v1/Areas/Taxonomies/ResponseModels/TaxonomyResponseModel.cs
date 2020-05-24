using Foundation.ApiExtensions.Models.Response;
using System;

namespace Api.v1.Areas.Taxonomies.ResponseModels
{
    public class TaxonomyResponseModel : BaseResponseModel
    {
        public Guid? Id { get; set; }
    }
}

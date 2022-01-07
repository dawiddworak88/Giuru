using Foundation.ApiExtensions.Models.Request;
using System;

namespace Catalog.Api.v1.Products.RequestModels
{
    public class RebuildCatalogIndexRequestModel : RequestModelBase
    {
        public Guid? SellerId { get; set; }
    }
}

using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class PagedClientFieldOptionsRequestModel : PagedRequestModelBase
    {
        public Guid? FieldDefinitionId { get; set; }
    }
}

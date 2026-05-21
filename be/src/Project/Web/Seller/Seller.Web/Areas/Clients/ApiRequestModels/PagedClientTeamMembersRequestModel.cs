using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class PagedClientTeamMembersRequestModel : PagedRequestModelBase
    {
        public Guid? OrganisationId { get; set; }
    }
}

using Foundation.PageContent.ComponentModels;
using System;

namespace Seller.Web.Areas.Clients.ComponentModels
{
    public class ClientTeamMemberComponentModel : ComponentModelBase
    {
        public Guid? OrganisationId { get; set; }
        public Guid? ClientId { get; set; }
    }
}
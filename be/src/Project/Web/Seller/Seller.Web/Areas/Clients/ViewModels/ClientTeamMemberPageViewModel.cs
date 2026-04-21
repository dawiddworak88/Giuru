using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.ViewModels;
using System;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientTeamMemberPageViewModel : BasePageViewModel
    {
        public Guid? Id { get; set; }
        public CatalogViewModel<ClientTeamMember> Catalog { get; set; }
    }
}

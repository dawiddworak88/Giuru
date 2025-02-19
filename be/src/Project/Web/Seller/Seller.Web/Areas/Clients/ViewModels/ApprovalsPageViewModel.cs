﻿using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ApprovalsPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<Approval> Catalog { get; set; }
    }
}

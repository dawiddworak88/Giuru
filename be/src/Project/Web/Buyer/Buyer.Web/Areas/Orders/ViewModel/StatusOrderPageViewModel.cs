﻿using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.Metadatas.ViewModels;

namespace Buyer.Web.Areas.Orders.ViewModel
{
    public class StatusOrderPageViewModel : BasePageViewModel
    {
        public MetadataViewModel Metadata { get; set; }
        public StatusOrderFormViewModel StatusOrder { get; set; }
    }
}

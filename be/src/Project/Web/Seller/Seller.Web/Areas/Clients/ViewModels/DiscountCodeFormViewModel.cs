using Foundation.PageContent.Components.ListItems.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class DiscountCodeFormViewModel
    {
        public Guid? Id { get; set; }

        /// <summary>The version of the code as it was loaded. Sent back to update it, so a stale form is refused.</summary>
        public string Version { get; set; }

        public string Title { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
        public IEnumerable<Guid> ClientIds { get; set; }
        public IEnumerable<ListItemViewModel> Clients { get; set; }
        public string CodeLabel { get; set; }
        public string CodeRequiredErrorMessage { get; set; }
        public string DescriptionLabel { get; set; }
        public string ClientsLabel { get; set; }
        public string NoClientsHint { get; set; }
        public string GrulaHint { get; set; }
        public string ActiveLabel { get; set; }
        public string InActiveLabel { get; set; }
        public string IdLabel { get; set; }
        public string SaveText { get; set; }
        public string SaveUrl { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string NavigateToDiscountCodes { get; set; }
        public string DiscountCodesUrl { get; set; }
    }
}

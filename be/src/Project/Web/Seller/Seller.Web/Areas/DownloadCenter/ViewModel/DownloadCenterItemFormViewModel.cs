using Foundation.PageContent.Components.ListItems.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.DownloadCenter.ViewModel
{
    public class DownloadCenterItemFormViewModel
    {
        public Guid? Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string Title { get; set; }
        public string SelectCategoryLabel { get; set; }
        public string SaveText { get; set; }
        public string NavigateToDownloadCenterLabel { get; set; }
        public string DownloadCenterUrl { get; set; }
        public string CategoryLabel { get; set; }
        public string IdLabel { get; set; }
        public string OrderLabel { get; set; }
        public string SaveUrl { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
        public int? Order { get; set; }
        public IEnumerable<ListItemViewModel> Categories { get; set; }
    }
}

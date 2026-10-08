using System;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class DiscountCodeFormViewModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string IdLabel { get; set; }
        public string CodeLabel { get; set; }
        public string Code { get; set; }
        public string DescriptionLabel { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
        public string ActiveLabel { get; set; }
        public string InActiveLabel { get; set; }
        public string DiscountCodesUrl { get; set; }
        public string NavigateToDiscountCodesText { get; set; }
        public string SaveText { get; set; }
        public string SaveUrl { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string CodeRequiredErrorMessage { get; set; }
        public string CodeMaxLengthErrorMessage { get; set; }
        public string DescriptionMaxLengthErrorMessage { get; set; }
    }
}

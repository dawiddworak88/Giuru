using System;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ApprovalFormViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string SaveUrl { get; set; }
        public string SaveText { get; set; }
        public string ClientApprovalsUrl { get; set; }
        public string NavigateToClientApprovals { get; set; }
        public string IdLabel { get; set; }
        public string NameLabel { get; set; }
        public string DescriptionLabel { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
    }
}

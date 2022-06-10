using System;

namespace Identity.Api.Configurations
{
    public class AppSettings
    {
        public string BuyerUrl { get; set; }
        public string MediaUrl { get; set; }
        public string IdentityUrl { get; set; }
        public string ClientUrl { get; set; }
        public Guid SellerClientId { get; set; }
        public string Regulations { get; set; }
        public string PrivacyPolicy { get; set; }
        public string DevelopersEmail { get; set; }
        public string ActionSendGridCreateTemplateId { get; set; }
        public string ActionSendGridResetTemplateId { get; set; }
        public string ActionSendGridClientApplyConfirmationTemplateId { get; set; }
        public string ActionSendGridClientApplyTemplateId { get; set; }
        public string ApplyRecipientEmail { get; set; }
    }
}

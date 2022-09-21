using System;

namespace Identity.Api.Configurations
{
    public class AppSettings
    {
        public string BuyerUrl { get; set; }
        public Guid SellerClientId { get; set; }
        public string Regulations { get; set; }
        public string PrivacyPolicy { get; set; }
        public string DevelopersEmail { get; set; }
        public string ActionSendGridCreateTemplateId { get; set; }
        public string ActionSendGridResetTemplateId { get; set; }
        public string ActionSendGridTeamMemberInvitationTemplateId { get; set; }
    }
}

using System;

namespace Identity.Api.Configurations
{
    public class AppSettings
    {
        public string ClientUrl { get; set; }
        public string BuyerUrl { get; set; }
        public Guid SellerClientId { get; set; }
        public string Regulations { get; set; }
        public string PrivacyPolicy { get; set; }
        public string DevelopersEmail { get; set; }
        public string ActionSendGridCreateTemplateId { get; set; }
        public string ActionSendGridResetTemplateId { get; set; }
        public string ActionSendGridTeamMemberInvitationTemplateId { get; set; }
        public string Email { get; set; }
        public Guid OrganisationId { get; set; }
        public string AppSecret { get; set; }
    }
}

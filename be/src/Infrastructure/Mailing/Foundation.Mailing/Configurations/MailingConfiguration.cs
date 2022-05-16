namespace Foundation.Mailing.Configurations
{
    public class MailingConfiguration
    {
        public string ApiKey { get; set; }
        public string NoReplyFromEmail { get; set; }
        public string ActionSendGridCreateTemplateId { get; set; }
        public string ActionSendGridResetTemplateId { get; set; }
        public string ActionSendGridClientApplyConfirmationTemplateId { get; set; }
        public string ActionSendGridClientApplyTemplateId { get; set; }
        public string DefaulEmail { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}

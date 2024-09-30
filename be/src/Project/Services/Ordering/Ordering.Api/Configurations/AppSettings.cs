namespace Ordering.Api.Configurations
{
    public class AppSettings
    {
        public string MediaUrl { get; set; }
        public string IdentityUrl { get; set; }
        public string ActionSendGridCustomOrderTemplateId { get; set; }
        public string ActionSendGridConfirmationOrderTemplateId { get; set; }
        public string ActionSendGridCancleOrderTemplateId { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}

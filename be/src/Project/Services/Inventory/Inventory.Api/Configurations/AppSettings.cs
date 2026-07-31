namespace Inventory.Api.Configurations
{
    public class AppSettings
    {
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string SendGridApiKey { get; set; }
        public string ActionSendGridProductOutOfStockTemplateId { get; set; }
        public string RecipientEmail { get; set; }
        public string CatalogUrl { get; set; }
    }
}

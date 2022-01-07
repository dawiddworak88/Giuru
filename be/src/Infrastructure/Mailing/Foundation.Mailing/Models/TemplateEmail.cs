namespace Foundation.Mailing.Models
{
    public class TemplateEmail
    {
        public string SenderName { get; set; }
        public string SenderEmailAddress { get; set; }
        public string RecipientName { get; set; }
        public string RecipientEmailAddress { get; set; }
        public string TemplateId { get; set; }
        public object DynamicTemplateData { get; set; }
    }
}

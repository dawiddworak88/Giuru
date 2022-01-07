namespace Foundation.Mailing.Models
{
    public class Email
    {
        public string SenderName { get; set; }
        public string SenderEmailAddress { get; set; }
        public string RecipientName { get; set; }
        public string RecipientEmailAddress { get; set; }
        public string Subject { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }
    }
}

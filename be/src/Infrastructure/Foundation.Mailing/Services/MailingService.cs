using Foundation.Mailing.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Foundation.Mailing.Services
{
    public class MailingService : IMailingService
    {
        private readonly ISendGridClient sendGridClient;

        public MailingService(ISendGridClient sendGridClient)
        {
            this.sendGridClient = sendGridClient;
        }

        public async Task SendAsync(Email email)
        {
            var from = new EmailAddress(email.SenderEmailAddress, email.SenderName);
            var to = new EmailAddress(email.RecipientEmailAddress, email.RecipientName);

            var msg = MailHelper.CreateSingleEmail(from, to, email.Subject, email.PlainTextContent, email.HtmlContent);

            await sendGridClient.SendEmailAsync(msg);
        }

        public async Task SendTemplateAsync(TemplateEmail email)
        {
            var from = new EmailAddress(email.SenderEmailAddress, email.SenderName);
            var to = new EmailAddress(email.RecipientEmailAddress, email.RecipientName);

            var msg = MailHelper.CreateSingleTemplateEmail(from, to, email.TemplateId, email.DynamicTemplateData);

            await sendGridClient.SendEmailAsync(msg);
        }
    }
}

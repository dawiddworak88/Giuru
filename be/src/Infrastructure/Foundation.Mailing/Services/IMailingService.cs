using Foundation.Mailing.Models;
using System.Threading.Tasks;

namespace Foundation.Mailing.Services
{
    public interface IMailingService
    {
        Task SendAsync(Email email);
        Task SendTemplateAsync(TemplateEmail email);
    }
}

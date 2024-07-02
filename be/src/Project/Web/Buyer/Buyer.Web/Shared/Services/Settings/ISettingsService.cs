using System;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Settings
{
    public interface ISettingsService
    {
        Task<bool> IsExternalCompletionDatesEnable(string token, string language, Guid? sellerId);
    }
}

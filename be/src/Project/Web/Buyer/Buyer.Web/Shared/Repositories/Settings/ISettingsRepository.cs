using Buyer.Web.Shared.DomainModels.Setting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Settings
{
    public interface ISettingsRepository
    {
        Task<Dictionary<string, string>> GetAsync(string token, string language, Guid? sellerId);
    }
}

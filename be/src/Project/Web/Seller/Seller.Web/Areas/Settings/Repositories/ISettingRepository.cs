using Seller.Web.Areas.Settings.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Settings.Repositories
{
    public interface ISettingRepository
    {
        Task<Dictionary<string, string>> GetAsync(string token, string language, Guid? sellerId);
        Task PostAsync(string token, string language, Guid? sellerId, Dictionary<string, string> settings);
    }
}

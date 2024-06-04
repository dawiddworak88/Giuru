using Global.Api.ServicesModels.Settings;
using System;
using System.Threading.Tasks;

namespace Global.Api.Services.Settings
{
    public interface ISettingsService
    {
        public Task SaveAsync(UpdateSettingServiceModel model);
        public SettingServiceModel Get(Guid sellerId);
    }
}

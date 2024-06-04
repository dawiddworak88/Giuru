using Global.Api.Infrastructure;
using Global.Api.ServicesModels.Settings;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Global.Api.Infrastructure.Entities.Settings;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Foundation.GenericRepository.Extensions;
using System;

namespace Global.Api.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private readonly GlobalContext _context;

        public SettingsService(GlobalContext context)
        {
            _context = context;
        }

        public SettingServiceModel Get(Guid sellerId)
        {
            var settings = _context.Settings.Where(x => x.IsActive && x.SellerId == sellerId);

            if (settings is not null)
            {
                return new SettingServiceModel
                {
                    Settings = settings.ToDictionary(x => x.Key, x => x.Value)
                };
            }

            return new SettingServiceModel
            {
                Settings = new Dictionary<string, string>()
            };
        }

        public async Task SaveAsync(UpdateSettingServiceModel model)
        {
            var existingSettings = await _context.Settings.Where(s => s.SellerId == model.SellerId).ToListAsync();

            foreach (var setting in model.Settings)
            {
                var existingSetting = existingSettings.FirstOrDefault(s => s.Key == setting.Key);

                if (existingSetting is not null)
                {
                    existingSetting.Value = setting.Value;
                }
                else
                {
                    _context.Settings.Add(new Setting { SellerId = model.SellerId, Key = setting.Key, Value = setting.Value }.FillCommonProperties());
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}

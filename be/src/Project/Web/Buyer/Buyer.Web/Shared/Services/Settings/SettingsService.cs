using Buyer.Web.Shared.Definitions.Settings;
using Buyer.Web.Shared.Repositories.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository _settingsRepository;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<bool> IsExternalCompletionDatesEnable(string token, string language, Guid? sellerId)
        {
            var settings = await _settingsRepository.GetAsync(token, language, sellerId);

            var externalCompletionDate = settings.GetValueOrDefault(SettingsConstants.ExternalCompletionDatesKey);

            return externalCompletionDate == "true";
        }
    }
}

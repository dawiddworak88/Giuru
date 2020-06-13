using Foundation.TenantDatabase.Areas.Translations.Entities;
using Foundation.TenantDatabase.Shared.Repositories;
using System;
using System.Linq;

namespace Foundation.TenantDatabase.Shared.Helpers
{
    public static class TranslationHelper
    {
        public static string Text(ITenantGenericRepository<Translation> translationRepository, Guid translationId, string language)
        {
            var translation = translationRepository.Get(x => x.Id == translationId && x.Language == language && x.IsActive).FirstOrDefault();

            if (translation == null)
            { 
                translation = translationRepository.Get(x => x.Id == translationId && x.IsActive).FirstOrDefault();
            }

            return translation?.Value;
        }

        public static string Text(ITenantGenericRepository<Translation> translationRepository, string key, string language)
        {
            var translation = translationRepository.Get(x => x.Key == key && x.Language == language && x.IsActive).FirstOrDefault();

            if (translation == null)
            {
                translation = translationRepository.Get(x => x.Key == key && x.IsActive).FirstOrDefault();
            }

            return translation?.Value;
        }
    }
}

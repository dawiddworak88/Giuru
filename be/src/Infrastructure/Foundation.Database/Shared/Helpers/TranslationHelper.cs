using Foundation.Database.Areas.Translations.Entities;
using Foundation.Database.Shared.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Database.Shared.Helpers
{
    public static class TranslationHelper
    {
        public static string Text(DatabaseContext context, Guid translationId, string language)
        {
            var translation = context.Translations.FirstOrDefault(x => x.Id == translationId && x.Language == language && x.IsActive);

            if (translation == null)
            { 
                translation = context.Translations.FirstOrDefault(x => x.Id == translationId && x.IsActive);
            }

            return translation?.Value;
        }

        public static string Text(IEnumerable<Translation> translations, Guid translationId, string language)
        {
            var translation = translations.FirstOrDefault(x => x.Id == translationId && x.Language == language && x.IsActive);

            if (translation == null)
            {
                translation = translations.FirstOrDefault(x => x.Id == translationId && x.IsActive);
            }

            return translation?.Value;
        }

        public static string Text(DatabaseContext context, string key, string language)
        {
            var translation = context.Translations.FirstOrDefault(x => x.Key == key && x.Language == language && x.IsActive);

            if (translation == null)
            {
                translation = context.Translations.FirstOrDefault(x => x.Key == key && x.IsActive);
            }

            return translation?.Value;
        }

        public static string Text(IEnumerable<Translation> translations, string key, string language)
        {
            var translation = translations.FirstOrDefault(x => x.Key == key && x.Language == language && x.IsActive);

            if (translation == null)
            {
                translation = translations.FirstOrDefault(x => x.Key == key && x.IsActive);
            }

            return translation?.Value;
        }
    }
}

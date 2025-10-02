using Buyer.Web.Areas.Products.Definitions;
using Buyer.Web.Areas.Products.Repositories.ProductAttributeItems;
using Buyer.Web.Shared.Configurations;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.ProductColors
{
    public class ProductColorsService : IProductColorsService
    {
        private readonly IDatabase _cache;
        private readonly IProductAttributeItemsRepository _productAttributeItemsRepository;
        private readonly IOptions<AppSettings> _options;

        public ProductColorsService(
            ConnectionMultiplexer redis,
            IProductAttributeItemsRepository productAttributeItemsRepository,
            IOptions<AppSettings> options)
        {
            _cache = redis.GetDatabase();
            _productAttributeItemsRepository = productAttributeItemsRepository;
            _options = options;
        }

        public async Task<string> ToEnglishAsync(string color)
        {
            await InitializeAsync();

            if (string.IsNullOrWhiteSpace(color))
            {
                return color;
            }

            var normalizedKey = NormalizeColorKey(color);
            var result = await _cache.HashGetAsync(ProductColorConstants.ColorCacheKey, normalizedKey);

            if (result.HasValue)
            {
                return result.ToString();
            }

            return color;
        }

        private async Task<IEnumerable<ColorTranslation>> FetchColorTranslationsAsync()
        {
            var colorsInPolishTranslation = await _productAttributeItemsRepository.GetAsync("pl", _options.Value.ProductColorAttributeId);
            var colorsInGermanTranslation = await _productAttributeItemsRepository.GetAsync("de", _options.Value.ProductColorAttributeId);
            var colorsInEnglishTranslation = await _productAttributeItemsRepository.GetAsync("en", _options.Value.ProductColorAttributeId);

            if (colorsInPolishTranslation == null || 
                colorsInGermanTranslation == null || 
                colorsInEnglishTranslation == null)
            {
                return Enumerable.Empty<ColorTranslation>();
            }

            var polishDictionary = colorsInPolishTranslation.ToDictionary(x => x.Id);
            var germanDictionary = colorsInGermanTranslation.ToDictionary(x => x.Id);
            var englishDictionary = colorsInEnglishTranslation.ToDictionary(x => x.Id);

            var keys = polishDictionary.Keys.Union(germanDictionary.Keys).Union(englishDictionary.Keys);

            return keys.Select(id => new ColorTranslation
            {
                Polish = polishDictionary.TryGetValue(id, out var p) ? p.Name : null,
                German = germanDictionary.TryGetValue(id, out var g) ? g.Name : null,
                English = englishDictionary.TryGetValue(id, out var e) ? e.Name : null
            });
        }

        private async Task InitializeAsync()
        {
            if (await _cache.KeyExistsAsync(ProductColorConstants.ColorCacheKey))
            {
                return;
            }

            var colorTranslations = await FetchColorTranslationsAsync();
            var hashEntries = new List<HashEntry>();

            foreach (var color in colorTranslations)
            {
                if (string.IsNullOrWhiteSpace(color.Polish) ||
                    string.IsNullOrWhiteSpace(color.German) ||
                    string.IsNullOrWhiteSpace(color.English))
                {
                    continue;
                }

                hashEntries.Add(new HashEntry(NormalizeColorKey(color.Polish), color.English));
                hashEntries.Add(new HashEntry(NormalizeColorKey(color.German), color.English));
                hashEntries.Add(new HashEntry(NormalizeColorKey(color.English), color.English));
            }

            await _cache.HashSetAsync(ProductColorConstants.ColorCacheKey, hashEntries.ToArray());
        }

        private static string NormalizeColorKey(string color)
        {
            if (string.IsNullOrWhiteSpace(color))
            {
                return color;
            }

            return color
                .Normalize(NormalizationForm.FormC)
                .Replace('\u00A0', ' ')
                .Trim()
                .ToLowerInvariant();
        }

        private class ColorTranslation
        {
            public string Polish { get; set; }
            public string German { get; set; }
            public string English { get; set; }
        }
    }
}

using Foundation.Extensions.ExtensionMethods;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.Configurations;
using System.Collections.Generic;
using System.Linq;

namespace Seller.Web.Shared.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly IOptions<AppSettings> _options;

        public ProductsService(IOptions<AppSettings> options)
        {
            _options = options;
        }

        public string GetFirstAvailableAttributeValue(IEnumerable<ReadProductAttribute> attributes, string possibleKeys)
        {
            var keys = possibleKeys.ToEnumerableString();

            foreach (var key in keys.OrEmptyIfNull())
            {
                var value = attributes.FirstOrDefault(x => x.Key == key)?.Values?.FirstOrDefault();

                if (string.IsNullOrWhiteSpace(value) is false)
                {
                    return value;
                }
            }

            return null;
        }

        public string GetSize(IEnumerable<ReadProductAttribute> attributes)
        {
            var widthValue = GetFirstAvailableAttributeValue(attributes, _options.Value.PossibleWidthAttributeKeys);
            var depthValue = GetFirstAvailableAttributeValue(attributes, _options.Value.PossibleDepthAttributeKeys);
            var lengthValue = GetFirstAvailableAttributeValue(attributes, _options.Value.PossibleLengthAttributeKeys);

            if (string.IsNullOrWhiteSpace(widthValue))
            {
                return default;
            }

            if (string.IsNullOrWhiteSpace(depthValue) is false)
            {
                var size = $"{widthValue}x{depthValue}".Trim();
                return size;
            }

            if (string.IsNullOrWhiteSpace(lengthValue) is false)
            {
                var size = $"{widthValue}x{lengthValue}".Trim();
                return size;
            }

            return default;
        }

        public string GetSleepAreaSize(IEnumerable<ReadProductAttribute> attributes)
        {
            var sleepAreaWidthValue = GetFirstAvailableAttributeValue(attributes, _options.Value.PossibleSleepAreaWidthAttributeKeys);
            var sleepAreaDepthValue = GetFirstAvailableAttributeValue(attributes, _options.Value.PossibleSleepAreaDepthAttributeKeys);

            if (string.IsNullOrWhiteSpace(sleepAreaWidthValue) ||
                string.IsNullOrWhiteSpace(sleepAreaDepthValue))
            {
                return default;
            }

            var size = $"{sleepAreaWidthValue}x{sleepAreaDepthValue}".Trim();

            return size;
        }
    }
}

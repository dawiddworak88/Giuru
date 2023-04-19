using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Foundation.Extensions.Formatters
{
    public static class CsvOutputFormatterMvcBuilderExtensions
    {
        public static IMvcBuilder AddCsvSerializerFormatters(this IMvcBuilder mvcBuilder)
        {
            if (mvcBuilder is null)
            {
                throw new ArgumentNullException(nameof(mvcBuilder));
            }

            mvcBuilder.Services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, CsvOutputFormatterSetup>()
            );

            return mvcBuilder;
        }
    }
}

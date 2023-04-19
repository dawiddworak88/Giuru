using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Foundation.Extensions.Formatters
{
    public class CsvOutputFormatterSetup : IConfigureOptions<MvcOptions>
    {
        void IConfigureOptions<MvcOptions>.Configure(MvcOptions options)
        {
            options.OutputFormatters.Add(new CsvOutputFormatter());
        }
    }
}

using Foundation.Search.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Foundation.Search.Binders
{
    public class SearchQueryFiltersBinder : IModelBinder
    {
        private static readonly Regex KeyPattern =
        new Regex(@"^search\[(?<prop>[a-zA-Z]+)\]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var f = new QueryFilters();

            foreach (var kv in bindingContext.HttpContext.Request.Query)
            {
                var match = KeyPattern.Match(kv.Key);
                if (!match.Success)
                    continue;

                var prop = match.Groups["prop"].Value.ToLowerInvariant();
                var values = kv.Value
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .Select(v => v.Trim())
                    .ToArray();

                if (values.Length == 0)
                    continue;

                switch (prop)
                {
                    case "category":
                        f.Category = values;
                        break;
                    case "shape":
                        f.Shape = values;
                        break;
                    case "color":
                        f.Color = values;
                        break;
                    case "height":
                        f.Height = values;
                        break;
                    case "width":
                        f.Width = values;
                        break;
                    case "depth":
                        f.Depth = values;
                        break;
                    default:
                        // nieznane pola ignorujemy
                        break;
                }
            }

            bindingContext.Result = ModelBindingResult.Success(f);

            return Task.CompletedTask;
        }
    }
}
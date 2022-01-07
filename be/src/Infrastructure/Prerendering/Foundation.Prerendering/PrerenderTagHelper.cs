using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Foundation.Prerendering
{
    [HtmlTargetElement(Attributes = PrerenderModuleAttributeName)]
    public class PrerenderTagHelper : TagHelper
    {
        private const string PrerenderModuleAttributeName = "asp-prerender-module";
        private const string PrerenderDataAttributeName = "asp-prerender-data";
        private const string PrerenderTimeoutAttributeName = "asp-prerender-timeout";

        public PrerenderTagHelper()
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        [HtmlAttributeName(PrerenderModuleAttributeName)]
        public string ModuleName { get; set; }

        [HtmlAttributeName(PrerenderDataAttributeName)]
        public object Parameters { get; set; }

        [HtmlAttributeName(PrerenderTimeoutAttributeName)]
        public int TimeoutMillisecondsParameter { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var endpointAddress = this.Configuration["ServerSideRenderingEndpoint"];

            if (!string.IsNullOrWhiteSpace(endpointAddress))
            {
                using (var client = new HttpClient())
                {
                    if (this.TimeoutMillisecondsParameter > 0)
                    {
                        client.Timeout = TimeSpan.FromMilliseconds(this.TimeoutMillisecondsParameter);
                    }

                    var request = new PrerenderRequest
                    { 
                        ModuleName = this.ModuleName,
                        Parameters = this.Parameters
                    };

                    var response = await client.PostAsync(
                        endpointAddress, 
                        new StringContent(JsonConvert.SerializeObject(request, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }), 
                        Encoding.UTF8, 
                        "application/json"));

                    ViewContext.HttpContext.Response.StatusCode = (int)response.StatusCode;

                    var result = await response.Content.ReadAsStringAsync();

                    output.Content.SetHtmlContent(result);
                }
            }
        }
    }
}

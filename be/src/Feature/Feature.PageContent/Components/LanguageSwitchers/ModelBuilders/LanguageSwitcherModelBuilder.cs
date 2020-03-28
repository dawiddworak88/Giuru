using Feature.PageContent.Components.LanguageSwitchers.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization.Definitions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;

namespace Feature.PageContent.Components.LanguageSwitchers.ModelBuilders
{
    public class LanguageSwitcherModelBuilder : IModelBuilder<LanguageSwitcherViewModel>
    {
        private readonly IOptionsMonitor<LocalizationConfiguration> localizationOptions;

        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly LinkGenerator linkGenerator;

        public LanguageSwitcherModelBuilder(IOptionsMonitor<LocalizationConfiguration> localizationOptions, IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            this.localizationOptions = localizationOptions;
            this.httpContextAccessor = httpContextAccessor;
            this.linkGenerator = linkGenerator;
        }

        public LanguageSwitcherViewModel BuildModel()
        {
            var languages = new List<LanguageViewModel>();

            foreach (var language in this.localizationOptions.CurrentValue.SupportedCultures)
            {
                var url = this.linkGenerator.GetPathByAction(this.httpContextAccessor.HttpContext.GetRouteValue("action").ToString(), this.httpContextAccessor.HttpContext.GetRouteValue("controller").ToString(), new { area = this.httpContextAccessor.HttpContext.GetRouteValue("area").ToString(), culture = language, id = this.httpContextAccessor.HttpContext.GetRouteValue("id")?.ToString() });

                if (this.httpContextAccessor.HttpContext.Request.QueryString.HasValue)
                {
                    url += this.httpContextAccessor.HttpContext.Request.QueryString.Value;
                }

                languages.Add(new LanguageViewModel { Text = language.ToUpperInvariant(), Url = url });
            }

            return new LanguageSwitcherViewModel
            {
                AvailableLanguages = languages,
                SelectedLanguageText = CultureInfo.CurrentUICulture.Name.ToUpperInvariant()
            };
        }
    }
}

using Foundation.PageContent.Components.Languages.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Localization.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using Seller.Web.Areas.Clients.ViewModels;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientFormModelBuilder : IModelBuilder<ClientFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly IOptionsMonitor<LocalizationConfiguration> localizationOptions;
        private readonly LinkGenerator linkGenerator;

        public ClientFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<ClientResources> clientLocalizer,
            IOptionsMonitor<LocalizationConfiguration> localizationOptions,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.clientLocalizer = clientLocalizer;
            this.localizationOptions = localizationOptions;
            this.linkGenerator = linkGenerator;
        }

        public ClientFormViewModel BuildModel()
        {
            var languages = new List<LanguageViewModel>
            { 
                new LanguageViewModel { Text = this.globalLocalizer["SelectLanguage"] , Value = string.Empty }
            };

            foreach (var language in this.localizationOptions.CurrentValue.SupportedCultures)
            {
                languages.Add(new LanguageViewModel { Text = language.ToUpperInvariant(), Value = language.ToLowerInvariant() });
            }

            return new ClientFormViewModel
            {
                GeneralErrorMessage = this.globalLocalizer["AnErrorOccurred"],
                ClientDetailText = this.clientLocalizer["Client"],
                NameLabel = this.globalLocalizer["NameLabel"],
                EmailLabel = this.globalLocalizer["EmailLabel"],
                LanguageLabel = this.globalLocalizer["CommunicationLanguageLabel"],
                NameRequiredErrorMessage = this.globalLocalizer["NameRequiredErrorMessage"],
                EmailRequiredErrorMessage = this.globalLocalizer["EmailRequiredErrorMessage"],
                EmailFormatErrorMessage = this.globalLocalizer["EmailFormatErrorMessage"],
                LanguageRequiredErrorMessage = this.globalLocalizer["LanguageRequiredErrorMessage"],
                EnterNameText = this.globalLocalizer["EnterNameText"],
                EnterEmailText = this.globalLocalizer["EnterEmailText"],
                SaveText = this.globalLocalizer["SaveText"],
                SaveUrl = this.linkGenerator.GetPathByAction("Create", "ClientApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                Languages = languages
            };
        }
    }
}

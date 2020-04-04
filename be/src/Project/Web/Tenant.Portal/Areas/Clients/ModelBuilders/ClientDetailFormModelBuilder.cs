using Feature.Client;
using Feature.PageContent.Components.Languages.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Localization.Definitions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Tenant.Portal.Areas.Clients.ViewModels;

namespace Tenant.Portal.Areas.Clients.ModelBuilders
{
    public class ClientDetailFormModelBuilder : IModelBuilder<ClientDetailFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly IOptionsMonitor<LocalizationConfiguration> localizationOptions;

        public ClientDetailFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer, 
            IStringLocalizer<ClientResources> clientLocalizer,
            IOptionsMonitor<LocalizationConfiguration> localizationOptions)
        {
            this.globalLocalizer = globalLocalizer;
            this.clientLocalizer = clientLocalizer;
            this.localizationOptions = localizationOptions;
        }

        public ClientDetailFormViewModel BuildModel()
        {
            var languages = new List<LanguageViewModel>
            { 
                new LanguageViewModel { Text = this.globalLocalizer["SelectLanguage"] , Value = string.Empty }
            };

            foreach (var language in this.localizationOptions.CurrentValue.SupportedCultures)
            {
                languages.Add(new LanguageViewModel { Text = language.ToUpperInvariant(), Value = language.ToUpperInvariant() });
            }

            return new ClientDetailFormViewModel
            {
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
                Languages = languages
            };
        }
    }
}

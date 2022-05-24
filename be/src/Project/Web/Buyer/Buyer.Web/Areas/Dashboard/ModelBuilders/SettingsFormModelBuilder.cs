using Buyer.Web.Areas.Dashboard.Repositories.Identity;
using Buyer.Web.Areas.Dashboard.ViewModel;
using Buyer.Web.Shared.Repositories.Clients;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Dashboard.ModelBuilders
{
    public class SettingsFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, SettingsFormViewModel>
    {
        private readonly LinkGenerator linkGenerator;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IIdentityRepository identityRepository;
        private readonly IClientsRepository clientRepository;

        public SettingsFormModelBuilder(
            LinkGenerator linkGenerator,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IClientsRepository clientRepository,
            IIdentityRepository identityRepository)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.identityRepository = identityRepository;
            this.clientRepository = clientRepository;
        }

        public async Task<SettingsFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new SettingsFormViewModel
            {
                GenerateAppSecretUrl = this.linkGenerator.GetPathByAction("Secret", "IdentityApi", new { Area = "Dashboard", culture = CultureInfo.CurrentUICulture.Name }),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                CopyLabel = this.globalLocalizer.GetString("Copy"),
                NameLabel = this.globalLocalizer.GetString("Name"),
                Name = componentModel.Name,
                AccountSettings = this.globalLocalizer.GetString("AccountSettings"),
                AccountData = this.globalLocalizer.GetString("AccountData"),
                EmailLabel = this.globalLocalizer.GetString("Email"),
                PhoneNumberLabel = this.globalLocalizer.GetString("PhoneNumberLabel"),
                ApiIdentifierTitle = this.globalLocalizer.GetString("ApiIdentifierTitle"),
                ApiIdentifierDescription = this.globalLocalizer.GetString("ApiIdentifierDescription"),
                OrganisationId = componentModel.SellerId,
                OrganisationLabel = this.globalLocalizer.GetString("OrganisationIdLabel"),
                AppSecretLabel = this.globalLocalizer.GetString("AppSecret"),
                GenerateText = this.globalLocalizer.GetString("Generate")
            };

            var appSecret = await this.identityRepository.GetSecretAsync(componentModel.Token, componentModel.Language);

            if (appSecret != null)
            {
                viewModel.AppSecret = appSecret;
            }

            var client = await this.clientRepository.GetClientAsync(componentModel.Token, componentModel.Language);

            if (client is not null)
            {
                viewModel.Email = client.Email;
                viewModel.PhoneNumber = client.PhoneNumber;
            }

            return viewModel;
        }
    }
}

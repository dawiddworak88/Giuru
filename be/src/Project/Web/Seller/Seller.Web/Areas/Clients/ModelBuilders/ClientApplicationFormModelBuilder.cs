using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.Repositories.Applications;
using Seller.Web.Areas.Clients.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientApplicationFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientApplicationFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly IClientApplicationsRepository clientApplicationsRepository;
        private readonly LinkGenerator linkGenerator;

        public ClientApplicationFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientApplicationsRepository clientApplicationsRepository,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.clientLocalizer = clientLocalizer;
            this.clientApplicationsRepository = clientApplicationsRepository;
            this.linkGenerator = linkGenerator;
        }

        public async Task<ClientApplicationFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientApplicationFormViewModel
            {
                Title = this.clientLocalizer.GetString("EditClientApplication"),
                IdLabel = this.globalLocalizer.GetString("Id"),
                FirstNameLabel = this.globalLocalizer.GetString("FirstName"),
                LastNameLabel = this.globalLocalizer.GetString("LastName"),
                EmailLabel = this.globalLocalizer.GetString("Email"),
                ContactJobTitleLabel = this.globalLocalizer.GetString("ContactJobTitle"),
                PhoneNumberLabel = this.globalLocalizer.GetString("PhoneNumberLabel"),
                CompanyNameLabel = this.globalLocalizer.GetString("CompanyName"),
                AddressLabel = this.globalLocalizer.GetString("Address"),
                CountryLabel = this.globalLocalizer.GetString("Country"),
                CityLabel = this.globalLocalizer.GetString("City"),
                RegionLabel = this.globalLocalizer.GetString("Region"),
                PostalCodeLabel = this.globalLocalizer.GetString("PostalCode"),
                BackToClientsApplicationsText = this.clientLocalizer.GetString("BackToClientsApplications"),
                ClientsApplicationsUrl = this.linkGenerator.GetPathByAction("Index", "ClientApplications", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                FieldRequiredErrorMessage = this.globalLocalizer.GetString("FieldRequiredErrorMessage"),
                EmailFormatErrorMessage = this.globalLocalizer.GetString("EmailFormatErrorMessage"),
                SaveUrl = this.linkGenerator.GetPathByAction("Post", "ClientsApplicationApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                SelectJobTitle = this.globalLocalizer.GetString("SelectJobTitle")
            };

            viewModel.ContactJobTitles = new List<ContactJobTitle>
            {
                new ContactJobTitle { 
                    Name = this.globalLocalizer.GetString("SalesRep").Name, 
                    Value = this.globalLocalizer.GetString("SalesRep").Value 
                },
                new ContactJobTitle { 
                    Name = this.globalLocalizer.GetString("SalesManager").Name, 
                    Value = this.globalLocalizer.GetString("SalesManager").Value
                },
                new ContactJobTitle { 
                    Name = this.globalLocalizer.GetString("President").Name, 
                    Value = this.globalLocalizer.GetString("President").Value 
                },
                new ContactJobTitle { 
                    Name = this.globalLocalizer.GetString("CEO").Name, 
                    Value = this.globalLocalizer.GetString("CEO").Value 
                },
                new ContactJobTitle { 
                    Name = this.globalLocalizer.GetString("AccountManager").Name, 
                    Value = this.globalLocalizer.GetString("AccountManager").Value 
                },
                new ContactJobTitle { 
                    Name = this.globalLocalizer.GetString("Owner").Name, 
                    Value = this.globalLocalizer.GetString("Owner").Value 
                },
                new ContactJobTitle { 
                    Name = this.globalLocalizer.GetString("VicePresident").Name, 
                    Value = this.globalLocalizer.GetString("VicePresident").Value 
                },
                new ContactJobTitle { 
                    Name = this.globalLocalizer.GetString("GeneralManager").Name, 
                    Value = this.globalLocalizer.GetString("GeneralManager").Value 
                },
                new ContactJobTitle { 
                    Name = this.globalLocalizer.GetString("OperationsManager").Name, 
                    Value = this.globalLocalizer.GetString("OperationsManager").Value 
                },
                new ContactJobTitle { 
                    Name = this.globalLocalizer.GetString("Other").Name, 
                    Value = this.globalLocalizer.GetString("Other").Value 
                }
            };

            if (componentModel.Id.HasValue)
            {
                var clientApplication = await this.clientApplicationsRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (clientApplication is not null)
                {
                    viewModel.Id = clientApplication.Id;
                    viewModel.FirstName = clientApplication.FirstName;
                    viewModel.LastName = clientApplication.LastName;
                    viewModel.Email = clientApplication.Email;
                    viewModel.ContactJobTitle = clientApplication.ContactJobTitle;
                    viewModel.PhoneNumber = clientApplication.PhoneNumber;
                    viewModel.CompanyName = clientApplication.CompanyName;
                    viewModel.CompanyAddress = clientApplication.CompanyAddress;
                    viewModel.CompanyCity = clientApplication.CompanyCity;
                    viewModel.CompanyCountry = clientApplication.CompanyCountry;
                    viewModel.CompanyRegion = clientApplication.CompanyRegion;
                    viewModel.CompanyPostalCode = clientApplication.CompanyPostalCode;
                }
            }

            return viewModel;
        }
    }
}
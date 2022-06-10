using Feature.Account;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Identity.Api.Areas.Accounts.ApiRequestModels;
using Identity.Api.Areas.Accounts.Repositories.Clients;
using Identity.Api.Areas.Accounts.Validators;
using Identity.Api.Services.Applications;
using Identity.Api.ServicesModels.Applications;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.ApiControllers
{
    [Area("Accounts")]
    public class ClientsApiController : BaseApiController
    {
        private readonly IStringLocalizer<AccountResources> accountLocalizer;
        private readonly IClientsRepository clientsRepository;
        private readonly IClientsApplicationsService clientsApplicationsService;

        public ClientsApiController(
            IStringLocalizer<AccountResources> accountLocalizer,
            IClientsApplicationsService clientsApplicationsService,
            IClientsRepository clientsRepository)
        {
            this.accountLocalizer = accountLocalizer;
            this.clientsRepository = clientsRepository;
            this.clientsApplicationsService = clientsApplicationsService;
        }

        [HttpPost]
        public async Task<IActionResult> Application([FromBody] ClientApplicationRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var serivceModel = new CreateClientApplicationServiceModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CompanyAddress = model.CompanyAddress,
                CompanyCity = model.CompanyCity,
                CompanyCountry = model.CompanyCountry,
                CompanyName = model.CompanyName,
                CompanyPostalCode = model.CompanyPostalCode,
                CompanyRegion = model.CompanyRegion,
                ContactJobTitle = model.ContactJobTitle
            };

            var validator = new CreateClientApplicationModelValidator();
            var validationResult = await validator.ValidateAsync(serivceModel);

            if (validationResult.IsValid)
            {
                await this.clientsRepository.CreateClientApplicationAsync(
                    token, language, model.FirstName, model.LastName, model.ContactJobTitle, model.Email, model.PhoneNumber, model.CompanyName,
                    model.CompanyAddress, model.CompanyCountry, model.CompanyCity, model.CompanyRegion, model.CompanyPostalCode);

                await this.clientsApplicationsService.CreateAsync(serivceModel);

                return this.StatusCode((int)HttpStatusCode.OK, new { Message = this.accountLocalizer.GetString("SuccessfullyClientApply").Value });
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}

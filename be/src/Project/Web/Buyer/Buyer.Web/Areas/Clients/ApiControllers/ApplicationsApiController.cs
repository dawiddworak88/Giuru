using Buyer.Web.Areas.Clients.Repositories;
using Buyer.Web.Shared.ApiRequestModels.Application;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    [AllowAnonymous]
    public class ApplicationsApiController : BaseApiController
    {
        private readonly IApplicationsRepository _applicationsRepository;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;

        public ApplicationsApiController(
            IApplicationsRepository applicationsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            _applicationsRepository = applicationsRepository;
            _globalLocalizer = globalLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Application([FromBody] ClientApplicationRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await _applicationsRepository.CreateClientApplicationAsync(
                token, language, model.FirstName, model.LastName, model.ContactJobTitle, model.Email, model.PhoneNumber, model.CompanyName,
                model.CompanyAddress, model.CompanyCountry, model.CompanyCity, model.CompanyRegion, model.CompanyPostalCode, model.CommunicationLanguage, model.IsDeliveryAddressEqualBillingAddress,
                model.BillingAddress, model.DeliveryAddress);

            return StatusCode((int)HttpStatusCode.OK, new { Message = _globalLocalizer.GetString("SuccessfullyClientApply").Value });
        }
    }
}

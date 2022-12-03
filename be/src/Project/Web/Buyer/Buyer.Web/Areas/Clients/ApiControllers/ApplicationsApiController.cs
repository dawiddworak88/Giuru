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
        private readonly IApplicationsRepository applicationsRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public ApplicationsApiController(
            IApplicationsRepository applicationsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.applicationsRepository = applicationsRepository;
            this.globalLocalizer = globalLocalizer;
        }

        [HttpPost]
        public async Task<IActionResult> Application([FromBody] ClientApplicationRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.applicationsRepository.CreateClientApplicationAsync(
                token, language, model.FirstName, model.LastName, model.ContactJobTitle, model.Email, model.PhoneNumber, model.CompanyName,
                model.CompanyAddress, model.CompanyCountry, model.CompanyCity, model.CompanyRegion, model.CompanyPostalCode);

            return StatusCode((int)HttpStatusCode.OK, new { Message = globalLocalizer.GetString("SuccessfullyClientApply").Value });
        }
    }
}

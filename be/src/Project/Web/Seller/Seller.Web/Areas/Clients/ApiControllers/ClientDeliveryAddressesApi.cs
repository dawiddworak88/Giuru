using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.Repositories.DeliveryAddresses;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ApiControllers
{
    [Area("Clients")]
    public class ClientDeliveryAddressesApi : BaseApiController
    {
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;
        private readonly IClientDeliveryAddressesRepository _deliveryAddressesRepository;

        public ClientDeliveryAddressesApi(
            IStringLocalizer<ClientResources> clientLocalizer,
            IClientDeliveryAddressesRepository deliveryAddressesRepository)
        {
            _clientLocalizer = clientLocalizer;
            _deliveryAddressesRepository = deliveryAddressesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] DeliveryAddressRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var deliveryAddressId = await _deliveryAddressesRepository.SaveAsync(token, language, model.Company, model.FirstName, model.LastName, model.PhoneNumber, model.Street, model.Region, model.PostCode, model.ClientId, model.CountryId);

            return this.StatusCode((int)HttpStatusCode.OK, new { Id = deliveryAddressId, Message = _clientLocalizer.GetString("DeliveryAddressSavedSuccessfully").Value });
        }
    }
}

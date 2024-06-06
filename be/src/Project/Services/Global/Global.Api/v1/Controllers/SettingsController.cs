using Global.Api.Services.Settings;
using Global.Api.ServicesModels.Settings;
using Global.Api.v1.RequestModels;
using Global.Api.v1.ResponseModels;
using Global.Api.validators.Settings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Global.Api.v1.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;
        
        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        /// <summary>
        /// Creates or updates settings.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Http code Ok.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SaveAsync(SettingRequestModel model)
        {
            var serviceModel = new UpdateSettingServiceModel
            {
                SellerId = model.SellerId,
                Settings = model.Settings
            };

            var validator = new UpdateSettingModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _settingsService.SaveAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            return StatusCode((int)HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Gets list of settings.
        /// </summary>
        /// <param name="sellerId">The seller id.</param>
        /// <returns>The list of settings.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{sellerId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SettingServiceModel))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAsync(Guid sellerId)
        {
            var serviceModel = new GetSettingsServiceModel
            {
                SellerId = sellerId,
            };

            var validator = new GetSettingsModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var settings = _settingsService.Get(sellerId);

                return StatusCode((int)HttpStatusCode.OK, settings);
            }

            return StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}

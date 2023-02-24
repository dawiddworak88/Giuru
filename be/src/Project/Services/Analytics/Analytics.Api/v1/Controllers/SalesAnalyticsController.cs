using Analytics.Api.Services.SalesAnalytics;
using Analytics.Api.ServicesModels.SalesAnalytics;
using Analytics.Api.v1.ResponseModels;
using Analytics.Api.Validators;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace Analytics.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class SalesAnalyticsController : BaseApiController
    {
        private readonly ISalesService _salesService;

        public SalesAnalyticsController(
            ISalesService salesService)
        {
            _salesService = salesService;
        }

        /// <summary>
        /// Get annual sales
        /// </summary>
        /// <returns>Annual sales.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult GetAnnualSales()
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetAnnualSalesServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                IsSeller = User.IsInRole("Seller")
            };

            var validator = new GetAnnualSalesModelValidator();
            var validationResult = validator.Validate(serviceModel);

            if (validationResult.IsValid)
            {
                var annualSales = _salesService.GetAnnualSales(serviceModel);

                if (annualSales is not null)
                {
                    var response = annualSales.Select(x => new AnnualSalesResponseModel
                    {
                        Month = x.Month,
                        Year = x.Year,
                        Quantity = x.Quantity
                    });

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Get countries sales
        /// </summary>
        /// <returns>Countries sales.</returns>
        [HttpGet("countries"), MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult GetCountriesSales()
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetCountriesSalesServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
            };

            var countrySales = _salesService.GetCountrySales(serviceModel);

            if (countrySales is not null)
            {
                var response = countrySales.Select(x => new CountrySalesResponseModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Quantity = x.Quantity
                });

                return this.StatusCode((int)HttpStatusCode.OK, response);
            }

            return this.StatusCode((int)HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Get daily sales
        /// </summary>
        /// <returns>Daily sales.</returns>
        [HttpGet("daily"), MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult GetDailySales()
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetDailySalesServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                IsSeller = User.IsInRole("Seller")
            };

            var validator = new GetDailySalesModelValidator();
            var validationResult = validator.Validate(serviceModel);

            if (validationResult.IsValid)
            {
                var dailySales = _salesService.GetDailySales(serviceModel);

                if (dailySales is not null)
                {
                    var response = dailySales.Select(x => new DailySalesResponseModel
                    {
                        Day = x.Day,
                        DayOfWeek = x.DayOfWeek,
                        Month = x.Month,
                        Year = x.Year,
                        Quantity = x.Quantity
                    });

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}

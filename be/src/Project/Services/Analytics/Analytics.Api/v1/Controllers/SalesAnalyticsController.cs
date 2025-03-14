﻿using Analytics.Api.Services.SalesAnalytics;
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
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

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
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>Annual sales.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult GetAnnualSales(DateTime? fromDate, DateTime? toDate)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetAnnualSalesServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                IsSeller = User.IsInRole("Seller"),
                FromDate = fromDate,
                ToDate = toDate
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
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>Countries sales.</returns>
        [HttpGet("countries"), MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult GetCountriesSales(DateTime? fromDate, DateTime? toDate)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetCountriesSalesServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                FromDate = fromDate,
                ToDate = toDate
            };

            var validator = new GetCountriesSalesModelValidator();
            var validationResult = validator.Validate(serviceModel);

            if (validationResult.IsValid)
            {
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
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Get daily sales
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>Daily sales.</returns>
        [HttpGet("daily"), MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult GetDailySales(DateTime? fromDate, DateTime? toDate)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetDailySalesServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                IsSeller = User.IsInRole("Seller"),
                FromDate = fromDate,
                ToDate = toDate
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

        /// <summary>
        /// Gets best selling products
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="size">Number of displayed products.</param>
        /// <returns>Best selling products</returns>
        [HttpGet("products"), MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult Get(DateTime? fromDate, DateTime? toDate, int? size)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetTopSalesProductsAnalyticsServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                IsSeller = this.User.IsInRole("Seller"),
                FromDate = fromDate,
                ToDate = toDate,
                Size = size
            };

            var validator = new GetTopSalesProductsAnalyticsModelValidator();
            var validationResult = validator.Validate(serviceModel);

            if (validationResult.IsValid)
            {
                var topSalesProducts = _salesService.GetTopProductsSales(serviceModel);

                if (topSalesProducts is not null)
                {
                    var response = topSalesProducts.Select(x => new TopSalesProductsAnalyticsResponseModel
                    {
                        ProductId = x.ProductId,
                        ProductSku = x.ProductSku,
                        ProductName = x.ProductName,
                        Ean = x.Ean,
                        Quantity = x.Quantity
                    });

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets best selling Clients
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="size">Number of displayed items.</param>
        /// <returns>Best selling clients</returns>
        [HttpGet("clients"), MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public IActionResult GetClientsSales(DateTime? fromDate, DateTime? toDate, int? size)
            {
                var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

                var serviceModel = new GetClientsSalesServiceModel
                {
                    Language = CultureInfo.CurrentCulture.Name,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    FromDate = fromDate,
                    ToDate = toDate,
                    Size = size
                };

                var validator = new GetClientsSalesModelValidator();
                var validationResult = validator.Validate(serviceModel);

                if (validationResult.IsValid)
                {
                    var clientsSales = _salesService.GetTopClientsSales(serviceModel);

                    if (clientsSales is not null)
                    {
                        var response = clientsSales.Select(x => new ClientsSalesResponseModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Quantity = x.Quantity
                        });

                        return this.StatusCode((int)HttpStatusCode.OK, response);
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }
}

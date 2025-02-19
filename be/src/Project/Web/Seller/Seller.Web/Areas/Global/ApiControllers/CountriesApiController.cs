﻿using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Definitions;
using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System;
using Seller.Web.Areas.Global.ApiRequestModels;
using Seller.Web.Areas.Global.DomainModels;
using Seller.Web.Areas.Global.Repositories;

namespace Seller.Web.Areas.Global.ApiControllers
{
    [Area("Global")]
    public class CountriesApiController : BaseApiController
    {
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly ICountriesRepository countriesRepository;

        public CountriesApiController(
            IStringLocalizer<ClientResources> clientLocalizer,
            ICountriesRepository countriesRepository)
        {
            this.clientLocalizer = clientLocalizer;
            this.countriesRepository = countriesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] CountryRequestModel model)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.countriesRepository.SaveAsync(token, language, model.Id, model.Name);

            return StatusCode((int)HttpStatusCode.OK, new { Message = clientLocalizer.GetString("CountrySavedSuccessfully").Value });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            await this.countriesRepository.DeleteAsync(token, language, id);

            return StatusCode((int)HttpStatusCode.OK, new { Message = clientLocalizer.GetString("CountryDeletedSuccessfully").Value });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm, int pageIndex, int itemsPerPage)
        {
            var token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var language = CultureInfo.CurrentUICulture.Name;

            var countries = await this.countriesRepository.GetAsync(
                token, language, searchTerm, pageIndex, itemsPerPage, $"{nameof(Country.CreatedDate)} desc");

            return this.StatusCode((int)HttpStatusCode.OK, countries);
        }
    }
}

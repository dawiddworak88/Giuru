﻿using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Clients.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    public class ClientCountriesController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientCountriesPageViewModel> clientCountriesPageModelBuilder;

        public ClientCountriesController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientCountriesPageViewModel> clientCountriesPageModelBuilder)
        {
            this.clientCountriesPageModelBuilder = clientCountriesPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Name = this.User.Identity.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.clientCountriesPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
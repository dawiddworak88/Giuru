﻿using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Clients.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    public class ClientApprovalController : Controller
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientApprovalPageViewModel> _clientApprovalPageModelBuilder;

        public ClientApprovalController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientApprovalPageViewModel> clientApprovalPageModelBuilder)
        {
            _clientApprovalPageModelBuilder = clientApprovalPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var compopnentModel = new ComponentModelBase
            {
                Id = id,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                Name = User.Identity.Name,
                IsAuthenticated = User.Identity.IsAuthenticated,
                SellerId = GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await _clientApprovalPageModelBuilder.BuildModelAsync(compopnentModel);

            return View(viewModel);
        }
    }
}

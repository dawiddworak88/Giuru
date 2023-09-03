using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Products.ComponentModels;
using Seller.Web.Areas.Products.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class CategoryController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CategoryPageViewModel> _categoryPageModelBuilder;
        private readonly IAsyncComponentModelBuilder<DuplicateCategoryComponentModel, CategoryPageViewModel> _duplicateCategoryPageModelBuilder;

        public CategoryController(
            IAsyncComponentModelBuilder<ComponentModelBase, CategoryPageViewModel> categoryPageModelBuilder,
            IAsyncComponentModelBuilder<DuplicateCategoryComponentModel, CategoryPageViewModel> duplicateCategoryPageModelBuilder)
        {
            _categoryPageModelBuilder = categoryPageModelBuilder;
            _duplicateCategoryPageModelBuilder = duplicateCategoryPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                IsAuthenticated = User.Identity.IsAuthenticated,
                Name = User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await _categoryPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }

        public async Task<IActionResult> Duplicate(Guid? id)
        {
            var componentModel = new DuplicateCategoryComponentModel
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await _duplicateCategoryPageModelBuilder.BuildModelAsync(componentModel);

            return View("Edit", viewModel);
        }
    }
}

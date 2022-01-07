using Buyer.Web.Shared.ViewModels.Breadcrumbs;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace Buyer.Web.Shared.ModelBuilders.Breadcrumbs
{
    public class BreadcrumbsModelBuilder<S, T> : IBreadcrumbsModelBuilder<S, T> where S : ComponentModelBase where T : BreadcrumbsViewModel, new()
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly LinkGenerator linkGenerator;

        public BreadcrumbsModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public T BuildModel(S componentModel)
        {
            var viewModel = new T
            { 
                Items = new List<BreadcrumbViewModel>
                { 
                    new BreadcrumbViewModel
                    { 
                        Name = this.globalLocalizer.GetString("Home"),
                        Url = this.linkGenerator.GetPathByAction("Index", "Home", new { Area = "Home", culture = CultureInfo.CurrentUICulture.Name }),
                        IsActive = false
                    }
                }
            };

            return viewModel;
        }
    }
}

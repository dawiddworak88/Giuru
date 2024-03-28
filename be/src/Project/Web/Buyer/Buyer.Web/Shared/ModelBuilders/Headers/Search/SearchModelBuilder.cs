using Buyer.Web.Shared.ViewModels.Headers.Search;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace Buyer.Web.Shared.ModelBuilders.Headers.Search
{
    public class SearchModelBuilder : IModelBuilder<SearchViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly LinkGenerator _linkGenerator;

        public SearchModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _linkGenerator = linkGenerator;
            
        }

        public SearchViewModel BuildModel()
        {
            var viewModel = new SearchViewModel
            {
                SearchTerm = string.Empty,
                SearchUrl = _linkGenerator.GetPathByAction("Index", "SearchProducts", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                SearchLabel = _globalLocalizer.GetString("Search"),
                SearchPlaceholderLabel = _globalLocalizer.GetString("Search"),
                GetSuggestionsUrl = _linkGenerator.GetPathByAction("Get", "SearchSuggestionsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                NoResultText = _globalLocalizer.GetString("NoResultText"),
                ChangeSearchTermText = _globalLocalizer.GetString("ChangeSearchTermText")
            };

            return viewModel;
        }
    }
}

using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.GraphQl;
using Buyer.Web.Shared.Repositories.GraphQl;
using Buyer.Web.Shared.ViewModels.Modals;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.Modals
{
    public class PriceModalModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, PriceModalViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IGraphQlRepository _graphQlRepository;
        private readonly IOptions<AppSettings> _options;

        public PriceModalModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IGraphQlRepository graphQlRepository,
            IOptions<AppSettings> options)
        {
            _globalLocalizer = globalLocalizer;
            _options = options;
            _graphQlRepository = graphQlRepository;
        }

        public async Task<PriceModalViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new PriceModalViewModel
            {
                Title = _globalLocalizer.GetString("PriceInclusions"),
                Note = await _graphQlRepository.GetTextAsync(componentModel.Language, _options.Value.DefaultCulture, GraphQlConstants.PriceModalNote)
            };

            return viewModel;
        }
        
    }
}
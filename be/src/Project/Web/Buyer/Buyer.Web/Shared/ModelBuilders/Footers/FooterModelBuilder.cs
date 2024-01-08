using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Repositories.GraphQl;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using System.Linq;
using Foundation.Extensions.ExtensionMethods;

namespace Buyer.Web.Shared.ModelBuilders.Footers
{
    public class FooterModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, FooterViewModel>
    {
        private readonly IGraphQlRepository _graphQlRepository;
        private readonly IOptions<AppSettings> _options;

        public FooterModelBuilder(
            IGraphQlRepository graphQlRepository,
            IOptions<AppSettings> options)
        {
            _graphQlRepository = graphQlRepository;
            _options = options;
        }

        public async Task<FooterViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new FooterViewModel();

            var footerContent = await _graphQlRepository.GetFooterAsync(componentModel.Language, _options.Value.DefaultCulture);

            if (footerContent is not null)
            {
                viewModel.Copyright = footerContent.Copyright;
                viewModel.Links = footerContent.Links.OrEmptyIfNull().Select(x => new LinkViewModel
                {
                    Url = x.Href,
                    Target = x.Target,
                    Text = x.Label
                });
            }

            return viewModel;
        }
    }
}

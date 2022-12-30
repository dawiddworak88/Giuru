using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;
using Identity.Api.Areas.Home.ViewModels;
using Identity.Api.Configurations;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Web;

namespace Identity.Api.Areas.Home.ModelBuilders
{
    public class RegulationsPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, RegulationsPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> _seoModelBuilder;
        private readonly IModelBuilder<HeaderViewModel> _headerModelBuilder;
        private readonly IModelBuilder<FooterViewModel> _footerModelBuilder;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IOptionsMonitor<AppSettings> _options;

        public RegulationsPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder,
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IOptionsMonitor<AppSettings> options)
        {
            _seoModelBuilder = seoModelBuilder;
            _headerModelBuilder = headerModelBuilder;
            _footerModelBuilder = footerModelBuilder;
            _globalLocalizer = globalLocalizer;
            _options = options;
        }

        public async Task<RegulationsPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new RegulationsPageViewModel
            {
                Metadata = await _seoModelBuilder.BuildModelAsync(componentModel),
                Header = _headerModelBuilder.BuildModel(),
                Content = new ContentPageViewModel
                { 
                    Title = _globalLocalizer.GetString("Regulations"),
                    Content = HttpUtility.UrlDecode(_options.CurrentValue.Regulations)
                },
                Footer = _footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}

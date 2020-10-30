using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Identity.Api.Areas.Home.Services.Contents;
using Identity.Api.Areas.Home.ViewModels;
using Identity.Api.Infrastructure.Contents.Definitions;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Home.ModelBuilders
{
    public class PrivacyPolicyPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, PrivacyPolicyPageViewModel>
    {
        private readonly IContentService contentService;
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public PrivacyPolicyPageModelBuilder(
            IContentService contentService,
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.contentService = contentService;
            this.headerModelBuilder = headerModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
            this.globalLocalizer = globalLocalizer;
        }

        public async Task<PrivacyPolicyPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new PrivacyPolicyPageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                Content = new ContentPageViewModel
                {
                    Title = this.globalLocalizer.GetString("PrivacyPolicy"),
                    Content = await this.contentService.GetAsync(ContentConstants.PrivacyPolicyId, componentModel.Language)
                },
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}

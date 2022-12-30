using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;
using Identity.Api.Areas.Home.Repositories.Content;
using Identity.Api.Areas.Home.ViewModels;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Home.ModelBuilders
{
    public class PrivacyPolicyPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, PrivacyPolicyPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> _seoModelBuilder;
        private readonly IModelBuilder<HeaderViewModel> _headerModelBuilder;
        private readonly IModelBuilder<FooterViewModel> _footerModelBuilder;
        private readonly IContentRepository _contentRepository;

        public PrivacyPolicyPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder,
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IContentRepository contentRepository)
        {
            _seoModelBuilder = seoModelBuilder;
            _headerModelBuilder = headerModelBuilder;
            _footerModelBuilder = footerModelBuilder;
            _contentRepository = contentRepository;
        }

        public async Task<PrivacyPolicyPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var content = await _contentRepository.GetContentAsync(componentModel.ContentPageKey, componentModel.Language);

            var viewModel = new PrivacyPolicyPageViewModel
            {
                Metadata = await _seoModelBuilder.BuildModelAsync(componentModel),
                Header = _headerModelBuilder.BuildModel(),
                Content = new ContentPageViewModel
                {
                    Title = content.Title,
                    Content = content.Text
                },
                Footer = _footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}

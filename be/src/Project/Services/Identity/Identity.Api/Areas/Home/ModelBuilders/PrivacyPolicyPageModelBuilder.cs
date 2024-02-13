using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;
using Identity.Api.Areas.Home.DomainModels;
using Identity.Api.Areas.Home.Repositories.Content;
using Identity.Api.Areas.Home.Repositories.Policy;
using Identity.Api.Areas.Home.ViewModels;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Home.ModelBuilders
{
    public class PrivacyPolicyPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, PrivacyPolicyPageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> _seoModelBuilder;
        private readonly IModelBuilder<HeaderViewModel> _headerModelBuilder;
        private readonly IModelBuilder<FooterViewModel> _footerModelBuilder;
        private readonly IPolicyRepository _policyRepository;

        public PrivacyPolicyPageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel> seoModelBuilder,
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder,
            IPolicyRepository policyRepository)
        {
            _seoModelBuilder = seoModelBuilder;
            _headerModelBuilder = headerModelBuilder;
            _footerModelBuilder = footerModelBuilder;
            _policyRepository = policyRepository;
        }

        public async Task<PrivacyPolicyPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var policy = await _policyRepository.GetPolicyAsync(componentModel.Language);

            var viewModel = new PrivacyPolicyPageViewModel
            {
                Metadata = await _seoModelBuilder.BuildModelAsync(componentModel),
                Header = _headerModelBuilder.BuildModel(),
                PrivacyPolicy = new PolicyDetailViewModel
                {
                    Title = policy.Title,
                    Description = policy.Description,
                    AccordionItems = policy.Accordions
                },
                Footer = _footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}

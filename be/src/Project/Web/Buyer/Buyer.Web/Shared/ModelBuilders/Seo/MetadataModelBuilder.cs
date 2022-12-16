using Buyer.Web.Shared.Repositories.Metadatas;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.Seo
{
    public class MetadataModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel>
    {
        private readonly IMetadataRepository metadataRepository;

        public MetadataModelBuilder(IMetadataRepository metadataRepository)
        {
            this.metadataRepository = metadataRepository;
        }

        public async Task<MetadataViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var metadata = await this.metadataRepository.GetMetadataAsync(componentModel.ContentPageKey, componentModel.Language);

            return new MetadataViewModel
            { 
                MetaTitle = metadata?.MetaTitle,
                MetaDescription = metadata?.MetaDescription
            };
        }
    }
}

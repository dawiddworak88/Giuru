using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;
using Foundation.PageContent.Repositories.Metadatas;
using System.Threading.Tasks;

namespace Foundation.PageContent.ModelBuilders.Metadatas
{
    public class MetadataModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, MetadataViewModel>
    {
        private readonly IMetadataRepository _metadataRepository;

        public MetadataModelBuilder(IMetadataRepository metadataRepository)
        {
            _metadataRepository = metadataRepository;
        }

        public async Task<MetadataViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var metadata = await _metadataRepository.GetMetadataAsync(componentModel.ContentPageKey, componentModel.Language);

            return new MetadataViewModel
            { 
                MetaTitle = metadata?.MetaTitle,
                MetaDescription = metadata?.MetaDescription
            };
        }
    }
}

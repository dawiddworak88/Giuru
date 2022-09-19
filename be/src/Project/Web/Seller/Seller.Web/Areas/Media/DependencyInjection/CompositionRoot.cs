using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Media.DomainModels;
using Seller.Web.Areas.Media.ModelBuilders;
using Seller.Web.Areas.Media.Repositories.Files;
using Seller.Web.Areas.Media.Repositories.Media;
using Seller.Web.Areas.Media.ViewModel;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Media.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterMediaAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IFilesRepository, FilesRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<MediaItem>>, MediaItemsPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, MediaItemsPageViewModel>, MediaItemsPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, MediaPageViewModel>, MediaFormPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, MediaFormViewModel>, MediaFormModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, MediaItemPageViewModel>, MediaItemPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, MediaItemFormViewModel>, MediaItemFormPageModelBuilder>();
        }
    }
}

using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.Media.DomainModels;
using Seller.Web.Areas.Media.ModelBuilders;
using Seller.Web.Areas.Media.Repositories;
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

            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<MediaItem>>, MediaPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, MediaPageViewModel>, MediaPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, UploadMediaPageViewModel>, UploadMediaPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, UploadMediaFormViewModel>, UploadMediaFormPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, EditMediaPageViewModel>, EditMediaPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, EditMediaFormViewModel>, EditMediaFormPageModelBuilder>();
        }
    }
}

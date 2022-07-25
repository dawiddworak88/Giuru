using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.DependencyInjection;
using Seller.Web.Areas.TeamMembers.DomainModels;
using Seller.Web.Areas.TeamMembers.ModelBuilders;
using Seller.Web.Areas.TeamMembers.ViewModel;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.TeamMembers.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterTeamMembersAreaDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<TeamMember>>, TeamMembersPageCatalogModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, TeamMembersPageViewModel>, TeamMembersPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, TeamMemberPageViewModel>, TeamMemberPageModelBuilder>();
            services.AddScoped<IAsyncComponentModelBuilder<ComponentModelBase, TeamMemberFormViewModel>, TeamMemberFormModelBuilder>();
        }
    }
}

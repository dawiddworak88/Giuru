using Buyer.Web.Shared.ViewModels.Breadcrumbs;
using Foundation.PageContent.ComponentModels;

namespace Buyer.Web.Shared.ModelBuilders.Breadcrumbs
{
    public interface IBreadcrumbsModelBuilder<in S, out T> where S : ComponentModelBase where T : BreadcrumbsViewModel, new()
    {
        T BuildModel(S componentModel);
    }
}

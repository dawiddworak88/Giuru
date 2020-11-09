using Buyer.Web.Shared.Breadcrumbs.ViewModels;
using Foundation.PageContent.ComponentModels;

namespace Buyer.Web.Shared.Breadcrumbs.ModelBuilders
{
    public interface IBreadcrumbsModelBuilder<in S, out T> where S : ComponentModelBase where T : BreadcrumbsViewModel, new()
    {
        T BuildModel(S componentModel);
    }
}

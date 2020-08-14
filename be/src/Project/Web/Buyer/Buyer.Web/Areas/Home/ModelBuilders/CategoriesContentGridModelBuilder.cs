using Buyer.Web.Areas.Home.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class CategoriesContentGridModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoriesContentGridViewModel>
    {
        public CategoriesContentGridModelBuilder()
        {
        }

        public async Task<CategoriesContentGridViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            return new CategoriesContentGridViewModel();
        }
    }
}

using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.HeroSliders.ViewModels;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class HeroSliderModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, HeroSliderViewModel>
    {
        public HeroSliderModelBuilder()
        { 
        }

        public async Task<HeroSliderViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            return new HeroSliderViewModel();
        }
    }
}

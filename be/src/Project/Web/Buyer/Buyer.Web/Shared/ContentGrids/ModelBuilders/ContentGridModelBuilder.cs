using Foundation.Extensions.ModelBuilders;
using System.Collections.Generic;
using Foundation.PageContent.Components.HeroSliders.ViewModels;
using Foundation.PageContent.Components.ContentGrids.ViewModels;

namespace Buyer.Web.Shared.Headers.ModelBuilders
{
    public class ContentGridModelBuilder : IModelBuilder<ContentGridViewModel>
    {
        public ContentGridModelBuilder()
        {
        }

        public ContentGridViewModel BuildModel()
        {
            return new ContentGridViewModel
            {
                Items = new List<ContentGridItemViewModel>
                {
                }
            };
        }
    }
}

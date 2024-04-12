using Buyer.Web.Areas.Content.ComponentModels;
using Buyer.Web.Areas.Content.DomainModel;
using Buyer.Web.Areas.Content.Repositories;
using Buyer.Web.Areas.Content.ViewModel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Content.ModelBuilders
{
    public class SlugPageContentWidgetsModelBuilder : IAsyncComponentModelBuilder<SlugContentComponentModel, SlugPageContentWidgetsViewModel>
    {
        private readonly ISlugRepository _slugRepository;

        public SlugPageContentWidgetsModelBuilder(
            ISlugRepository slugRepository)
        {
            _slugRepository = slugRepository;
        }

        public async Task<SlugPageContentWidgetsViewModel> BuildModelAsync(SlugContentComponentModel componentModel)
        {
            var viewModel = new SlugPageContentWidgetsViewModel
            {
                Title = "asdasd",
            };

            var slugPage = await _slugRepository.GetPageBySlugAsync(componentModel.Language, "en", componentModel.Slug);

            if (slugPage is not null)
            {
                viewModel.Title = slugPage.Title;

                var widgets = new List<WidgetViewModel>();

                foreach (var block in slugPage.Blocks.OrEmptyIfNull())
                {
                    if (block is SliderBlockPage sliderBlock)
                    {

                    }
                    else if (block is ContentBlockPage contentBlock)
                    {
                        var contentWidget = new ContentWidgetViewModel
                        {
                            Typename = contentBlock.Typename,
                            Content = contentBlock.Content
                        };

                        widgets.Add(contentWidget);
                    }
                }

                viewModel.Widgets = widgets;
            }

            return viewModel;
        }
    }
}

using AspNetCore.Extensions.ModelBuilders;
using AspNetCore.Shared.Headers.ViewModels;

namespace AspNetCore.Shared.Headers.ModelBuilders
{
    public class HeaderModelBuilder : IModelBuilder<HeaderViewModel>
    {
        private readonly IModelBuilder<LogoViewModel> logoModelBuilder;

        public HeaderModelBuilder(IModelBuilder<LogoViewModel> logoModelBuilder)
        {
            this.logoModelBuilder = logoModelBuilder;
        }

        public HeaderViewModel BuildModel()
        {
            return new HeaderViewModel
            {
                Logo = this.logoModelBuilder.BuildModel()
            };
        }
    }
}

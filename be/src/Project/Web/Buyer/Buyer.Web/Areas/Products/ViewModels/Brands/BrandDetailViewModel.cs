using Buyer.Web.Shared.ViewModels.Files;

namespace Buyer.Web.Areas.Products.ViewModels.Brands
{
    public class BrandDetailViewModel
    {
        public string LogoUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public FilesViewModel Files { get; set; }
    }
}

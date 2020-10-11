using Buyer.Web.Shared.Files.ViewModels;

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

using Buyer.Web.Areas.Products.DomainModels;
using Foundation.Pricing.DomainModels;
using Buyer.Web.Shared.ViewModels.Catalogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Prices
{
    // Every PriceProduct is built here so the Outlet price driver can never be
    // omitted or diverge from the line's actual purchase intent across call sites.
    public interface IPriceProductFactory
    {
        Task<PriceProduct> CreateAsync(Product product, bool isOutletPurchase);

        Task<IEnumerable<PriceProduct>> CreateAsync(IEnumerable<Product> products, bool isOutletPurchase);

        PriceProduct Create(CatalogItemViewModel product, bool isOutletPurchase);
    }
}

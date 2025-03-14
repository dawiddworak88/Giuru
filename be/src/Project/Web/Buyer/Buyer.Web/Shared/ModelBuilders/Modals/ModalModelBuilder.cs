﻿using Buyer.Web.Shared.ViewModels.Modals;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.Modal
{
    public class ModalModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel>
    {
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ModalModelBuilder(
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            this.orderLocalizer = orderLocalizer;
            this.globalLocalizer = globalLocalizer;
            this.linkGenerator = linkGenerator;
            this.productLocalizer = productLocalizer;
        }

        public async Task<ModalViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ModalViewModel
            {
                Title = this.orderLocalizer.GetString("AddingToCart"),
                MaximalLabel = this.globalLocalizer.GetString("MaximalLabel"),
                OkLabel = this.globalLocalizer.GetString("Ok"),
                MoreInfoLabel = this.orderLocalizer.GetString("MoreInfoLabel"),
                ExternalReferenceLabel = this.orderLocalizer.GetString("ExternalReferenceLabel"),
                DeliveryFromLabel = this.orderLocalizer.GetString("DeliveryFrom"),
                DeliveryToLabel = this.orderLocalizer.GetString("DeliveryTo"),
                QuantityLabel = this.orderLocalizer.GetString("QuantityLabel"),
                ToBasketText = this.globalLocalizer.GetString("BasketLabel"),
                StockQuantityLabel = this.orderLocalizer.GetString("StockQuantityLabel"),
                OutletQuantityLabel = this.orderLocalizer.GetString("OutletQuantityLabel"),
                BasketLabel = this.globalLocalizer.GetString("Basket"),
                BasketUrl = this.linkGenerator.GetPathByAction("Index", "Order", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                SkuLabel = this.productLocalizer.GetString("Sku"),
                EanLabel = this.globalLocalizer.GetString("Ean"),
                InBasket = this.globalLocalizer.GetString("InBasket")
            };

            return viewModel;
        }
    }
}

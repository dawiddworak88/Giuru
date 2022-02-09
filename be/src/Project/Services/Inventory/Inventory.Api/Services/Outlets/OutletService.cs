using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Inventory.Api.Infrastructure;
using Inventory.Api.Infrastructure.Entities;
using Inventory.Api.ServicesModels.OutletServices;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Api.Services.Outlets
{
    public class OutletService : IOutletService
    {
        private readonly InventoryContext context;
        private readonly IStringLocalizer inventortLocalizer;

        public OutletService(
            InventoryContext context,
            IStringLocalizer<InventoryResources> inventortLocalizer)
        {
            this.context = context;
            this.inventortLocalizer = inventortLocalizer;
        }

        public async Task<OutletServiceModel> SyncOutletAsync(OutletServiceModel model)
        {
            var outletItems = this.context.Outlet.Select(x => new OutletItemServiceModel
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProductSku = x.ProductSku,
            });

            foreach (var item in model.OutletItems.OrEmptyIfNull())
            {
                var outletItem = outletItems.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (outletItem is null)
                {
                    var outlet = new Outlet
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        ProductSku = item.ProductSku,
                    };

                    this.context.Outlet.Add(outlet.FillCommonProperties());
                }
                else
                {
                    foreach (var item2 in outletItems)
                    {
                        if (item2.ProductId.Value != item.ProductId)
                        {
                            Console.WriteLine("TAKI ITEM NIE ZOSTAŁ PRZESŁANY");
                        }
                    }
                }

            }

            await this.context.SaveChangesAsync();

            return new OutletServiceModel
            {
                OutletItems = model.OutletItems,
            };
        }

        public async Task<PagedResults<IEnumerable<OutletItemServiceModel>>> GetAsync(GetOutletsServiceModel model)
        {
            var outletItems = this.context.Outlet.Select(x => new OutletItemServiceModel
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProductSku = x.ProductSku,
                LastModifiedDate = x.LastModifiedDate,
                CreatedDate = x.CreatedDate
            });

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                outletItems = outletItems.Where(x => x.ProductName.StartsWith(model.SearchTerm) || x.ProductSku.StartsWith(model.SearchTerm));
            }

            outletItems = outletItems.ApplySort(model.OrderBy);

            return outletItems.PagedIndex(new Pagination(outletItems.Count(), model.ItemsPerPage), model.PageIndex);
        }
    }
}

using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Inventory.Api.Infrastructure;
using Inventory.Api.Infrastructure.Entities;
using Inventory.Api.ServicesModels.OutletServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public async Task<SyncOutletServiceModel> SyncOutletAsync(SyncOutletServiceModel model)
        {
            var outletItems = this.context.Outlet.Select(x => new Outlet
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProductSku = x.ProductSku,
            });

            var syncItems = new List<Outlet>();
            foreach (var item in model.OutletItems.OrEmptyIfNull())
            {
                var outletItem = new Outlet
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductSku = item.ProductSku,
                };

                var existingItem = outletItems.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (existingItem is null)
                {
                    this.context.Outlet.Add(outletItem.FillCommonProperties());
                }

                syncItems.Add(outletItem);
            }

            var soldItems = outletItems.ToList().Where(x => !syncItems.Any(s => s.ProductId == x.ProductId));

            this.context.RemoveRange(soldItems);
            await this.context.SaveChangesAsync();

            return new SyncOutletServiceModel
            {
                OutletItems = model.OutletItems,
            };
        }

        public async Task<PagedResults<IEnumerable<SyncOutletItemServiceModel>>> GetAsync(GetOutletsServiceModel model)
        {
            var outletItems = this.context.Outlet.Where(x => x.IsActive)
                .Select(x => new SyncOutletItemServiceModel
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

        public async Task DeleteAsync(DeleteOutletServiceModel model)
        {
            var outletItem = await this.context.Outlet.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);
            if (outletItem == null)
            {
                throw new CustomException(this.inventortLocalizer.GetString("OutletNotFound"), (int)HttpStatusCode.NotFound);
            }

            outletItem.IsActive = false;
            await this.context.SaveChangesAsync();
        }

        public async Task<Guid> CreateAsync(OutletServiceModel model)
        {
            var outletItem = new Outlet
            {
                ProductId = model.ProductId,
                ProductName = model.ProductName,
                ProductSku = model.ProductSku,
            };

            this.context.Outlet.Add(outletItem.FillCommonProperties());
            await this.context.SaveChangesAsync();

            return outletItem.Id;
        }

        public async Task<OutletServiceModel> GetAsync(GetOutletServiceModel model)
        {
            var outletItem = from o in this.context.Outlet
                             where o.Id == model.Id.Value && o.IsActive
                             select new OutletServiceModel
                             {
                                 Id = model.Id.Value,
                                 ProductId = o.ProductId,
                                 ProductName= o.ProductName,
                                 ProductSku= o.ProductSku,
                                 LastModifiedDate = o.LastModifiedDate,
                                 CreatedDate = o.CreatedDate
                             };

            return await outletItem.FirstOrDefaultAsync();
        }

        public async Task<Guid> UpdateAsync(UpdateOutletServiceModel model)
        {
            var outletItem = await this.context.Outlet.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);
            if (outletItem is null)
            {
                throw new CustomException(this.inventortLocalizer.GetString("InventoryNotFound"), (int)HttpStatusCode.NotFound);
            }

            outletItem.ProductId = model.ProductId.Value;
            outletItem.ProductName = model.ProductName;
            outletItem.ProductSku = model.ProductSku;
            outletItem.LastModifiedDate = DateTime.UtcNow;

            await this.context.SaveChangesAsync();
            
            return outletItem.Id;
        }
    }
}

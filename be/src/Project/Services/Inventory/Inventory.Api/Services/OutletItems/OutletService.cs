using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Inventory.Api.Infrastructure;
using Inventory.Api.Infrastructure.Entities;
using Inventory.Api.ServicesModels.OutletServiceModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;

namespace Inventory.Api.Services.OutletItems
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

        public async Task<OutletServiceModel> UpdateAsync(UpdateOutletServiceModel model)
        {
            var outletProduct = await this.context.Outlet.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);
            
            if (outletProduct == null)
            {
                throw new CustomException(this.inventortLocalizer.GetString("OutletNotFound"), (int)HttpStatusCode.NotFound);
            }

            outletProduct.WarehouseId = model.WarehouseId.Value;
            outletProduct.ProductId = model.ProductId.Value;
            outletProduct.ProductName = model.ProductName;
            outletProduct.ProductSku = model.ProductSku;
            outletProduct.Quantity = model.Quantity;
            outletProduct.AvailableQuantity = model.AvailableQuantity;
            outletProduct.LastModifiedDate = DateTime.UtcNow;

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetOutletServiceModel { Id = outletProduct.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }

        public async Task<OutletServiceModel> CreateAsync(CreateOutletServiceModel model)
        {
            var outletProduct = new OutletItem
            {
                WarehouseId = model.WarehouseId.Value,
                ProductId = model.ProductId.Value,
                ProductName = model.ProductName,
                ProductSku = model.ProductSku,
                Quantity = model.Quantity,
                AvailableQuantity = model.AvailableQuantity,
                SellerId = model.OrganisationId.Value
            };

            this.context.Outlet.Add(outletProduct.FillCommonProperties());

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetOutletServiceModel { Id = outletProduct.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }

        public async Task SyncOutletProducts(UpdateOutletProductsServiceModel model)
        {
            var outletProducts = this.context.Outlet
                .Join(this.context.Warehouses, outlet => outlet.WarehouseId, warehouse => warehouse.Id, (outlet, warehouse) => 
                new
                {
                    Id = outlet.Id,
                    ProductSku = outlet.ProductSku,
                    WarehouseName = warehouse.Name,
                });


            foreach (var item in model.OutletItems.OrEmptyIfNull())
            {
                var outlet = await outletProducts.FirstOrDefaultAsync(x => x.ProductSku == item.ProductSku && x.WarehouseName == item.WarehouseName);

                if (outlet is not null)
                {
                    var outletItem = await this.context.Outlet.FirstOrDefaultAsync(x => x.Id == outlet.Id);

                    outletItem.AvailableQuantity = item.AvailableQuantity;
                    outletItem.Quantity = item.Quantity;
                    outletItem.LastModifiedDate = DateTime.UtcNow;
                }
                else
                {
                    var warehouse = await this.context.Warehouses.FirstOrDefaultAsync(x => x.Name == item.WarehouseName);

                    if (warehouse is not null)
                    {
                        var outletProduct = new OutletItem
                        {
                            WarehouseId = warehouse.Id,
                            ProductId = item.ProductId.Value,
                            ProductName = item.ProductName,
                            ProductSku = item.ProductSku,
                            Quantity = item.Quantity,
                            AvailableQuantity = item.AvailableQuantity,
                            SellerId = model.OrganisationId.Value
                        };

                        this.context.Outlet.Add(outletProduct.FillCommonProperties());
                    }
                }
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<OutletServiceModel> GetAsync(GetOutletServiceModel model)
        {
            var outletProduct = from c in this.context.Outlet
                                   join warehouse in this.context.Warehouses on c.WarehouseId equals warehouse.Id
                                   where c.Id == model.Id.Value && c.IsActive
                                   select new OutletServiceModel
                                   {
                                       Id = c.Id,
                                       ProductId = c.ProductId,
                                       ProductName = c.ProductName,
                                       ProductSku = c.ProductSku,
                                       WarehouseId = c.WarehouseId,
                                       WarehouseName = warehouse.Name,
                                       Quantity = c.Quantity,
                                       AvailableQuantity = c.AvailableQuantity,
                                       LastModifiedDate = c.LastModifiedDate,
                                       CreatedDate = c.CreatedDate
                                   };

            return await outletProduct.FirstOrDefaultAsync();
        }

        public async Task<PagedResults<IEnumerable<OutletServiceModel>>> GetAsync(GetOutletsServiceModel model)
        {
            var outlets = from c in this.context.Outlet
                              join warehouse in this.context.Warehouses on c.WarehouseId equals warehouse.Id
                              where c.SellerId == model.OrganisationId.Value && c.IsActive
                              select new OutletServiceModel
                              {
                                Id = c.Id,
                                ProductId = c.ProductId,
                                ProductName = c.ProductName,
                                ProductSku = c.ProductSku,
                                WarehouseId = c.WarehouseId,
                                WarehouseName = warehouse.Name,
                                Quantity = c.Quantity,
                                AvailableQuantity = c.AvailableQuantity,
                                LastModifiedDate = c.LastModifiedDate,
                                CreatedDate = c.CreatedDate
                              };

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                outlets = outlets.Where(x => x.ProductName.StartsWith(model.SearchTerm) || x.WarehouseName.StartsWith(model.SearchTerm) || x.ProductSku.StartsWith(model.SearchTerm));
            }

            outlets = outlets.ApplySort(model.OrderBy);

            return outlets.PagedIndex(new Pagination(outlets.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<PagedResults<IEnumerable<OutletServiceModel>>> GetByIdsAsync(GetOutletsByIdsServiceModel model)
        {
            var outletProducts = from c in this.context.Outlet
                             join warehouse in this.context.Warehouses on c.WarehouseId equals warehouse.Id
                             where model.Ids.Contains(c.Id) && c.SellerId == model.OrganisationId.Value && c.IsActive
                             select new OutletServiceModel
                             {
                                 Id = c.Id,
                                 ProductId = c.ProductId,
                                 ProductName = c.ProductName,
                                 ProductSku = c.ProductSku,
                                 WarehouseId = c.WarehouseId,
                                 WarehouseName = warehouse.Name,
                                 LastModifiedDate = c.LastModifiedDate,
                                 CreatedDate = c.CreatedDate
                             };

            return outletProducts.PagedIndex(new Pagination(outletProducts.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<OutletSumServiceModel> GetOutletByProductId(GetOutletByProductIdServiceModel model)
        {
            var outlet = from i in this.context.Outlet
                            join warehouse in this.context.Warehouses on i.WarehouseId equals warehouse.Id
                            where i.ProductId == model.ProductId.Value && i.IsActive
                            select new
                            {
                                Id = i.Id,
                                ProductId = i.ProductId,
                                ProductName = i.ProductName,
                                ProductSku = i.ProductSku,
                                Quantity = i.Quantity,
                                AvailableQuantity = i.AvailableQuantity,
                                WarehouseId = i.WarehouseId,
                                WarehouseName = warehouse.Name,
                                LastModifiedDate = i.LastModifiedDate,
                                CreatedDate = i.CreatedDate
                            };

            if (outlet.OrEmptyIfNull().Any())
            {
                var outletSum = new OutletSumServiceModel
                {
                    ProductId = model.ProductId.Value,
                    ProductName = outlet.FirstOrDefault().ProductName,
                    ProductSku = outlet.FirstOrDefault().ProductSku,
                    AvailableQuantity = outlet.Sum(x => x.AvailableQuantity),
                    Quantity = outlet.Sum(x => x.Quantity),
                    Details = outlet.Select(item => new OutletServiceModel
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        ProductSku = item.ProductSku,
                        AvailableQuantity = item.AvailableQuantity,
                        Quantity = item.Quantity,
                        WarehouseId = item.WarehouseId,
                        WarehouseName = item.WarehouseName,
                        LastModifiedDate = item.LastModifiedDate,
                        CreatedDate = item.CreatedDate
                    })
                };

                return outletSum;
            }

            return default;                
        }

        public async Task<OutletSumServiceModel> GetOutletByProductSku(GetOutletByProductSkuServiceModel model)
        {
            var outlet = from i in this.context.Outlet
                            join warehouse in this.context.Warehouses on i.WarehouseId equals warehouse.Id
                            where i.ProductSku == model.ProductSku && i.IsActive
                            select new
                            {
                                Id = i.Id,
                                ProductId = i.ProductId,
                                ProductName = i.ProductName,
                                ProductSku = i.ProductSku,
                                Quantity = i.Quantity,
                                AvailableQuantity = i.AvailableQuantity,
                                WarehouseId = i.WarehouseId,
                                WarehouseName = warehouse.Name,
                                LastModifiedDate = i.LastModifiedDate,
                                CreatedDate = i.CreatedDate
                            };

            if (outlet.OrEmptyIfNull().Any())
            {
                var outletSum = new OutletSumServiceModel
                {
                    ProductId = outlet.FirstOrDefault().ProductId,
                    ProductName = outlet.FirstOrDefault().ProductName,
                    ProductSku = model.ProductSku,
                    AvailableQuantity = outlet.Sum(x => x.AvailableQuantity),
                    Quantity = outlet.Sum(x => x.Quantity),
                    Details = outlet.Select(item => new OutletServiceModel
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        ProductSku = item.ProductSku,
                        AvailableQuantity = item.AvailableQuantity,
                        Quantity = item.Quantity,
                        WarehouseId = item.WarehouseId,
                        WarehouseName = item.WarehouseName,
                        LastModifiedDate = item.LastModifiedDate,
                        CreatedDate = item.CreatedDate
                    })
                };

                return outletSum;
            }

            return default;
        }

        public async Task UpdateOutletProduct(Guid? productId, string productName, string productSku, Guid? organisationId)
        {
            var outletProduct = await this.context.Outlet.FirstOrDefaultAsync(x => x.ProductId == productId.Value && x.SellerId == organisationId.Value && x.IsActive);
            if (outletProduct != null)
            {
                outletProduct.ProductName = productName;
                outletProduct.ProductSku = productSku;
                outletProduct.LastModifiedDate = DateTime.UtcNow;

                await this.context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(DeleteOutletServiceModel model)
        {
            var outlet = await this.context.Outlet.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);
            if (outlet == null)
            {
                throw new CustomException(this.inventortLocalizer.GetString("OutletNotFound"), (int)HttpStatusCode.NotFound);
            }
            outlet.IsActive = false;
            await this.context.SaveChangesAsync();
        }

        public async Task<PagedResults<IEnumerable<OutletSumServiceModel>>> GetAvailableProductsOutletsAsync(GetOutletsServiceModel model)
        {
            var outlets = (from i in this.context.Outlet
                            group i by new { i.ProductId } into gpi
                            where gpi.Sum(x => x.AvailableQuantity) > 0
                            select new OutletSumServiceModel
                            {
                                ProductId = gpi.Key.ProductId,
                                ProductName = gpi.FirstOrDefault().ProductName,
                                ProductSku = gpi.FirstOrDefault().ProductSku,
                                AvailableQuantity = gpi.Sum(x => x.AvailableQuantity),
                                Quantity = gpi.Sum(x => x.Quantity)
                            }).OrderByDescending(x => x.AvailableQuantity);

                return outlets.PagedIndex(new Pagination(outlets.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task UpdateOutletBasket(Guid? ProductId, int BookedQuantity)
        {
            var outletProduct = this.context.Outlet.FirstOrDefault(x => x.ProductId == ProductId.Value && x.IsActive);
            if (outletProduct != null)
            {
                var productQuantity = outletProduct.Quantity + BookedQuantity;

                if (productQuantity < 0)
                {
                    productQuantity = 0;
                }

                outletProduct.Quantity = productQuantity;
                outletProduct.AvailableQuantity = productQuantity;
                outletProduct.LastModifiedDate = DateTime.UtcNow;

                await this.context.SaveChangesAsync();
            }
        }
    }
}

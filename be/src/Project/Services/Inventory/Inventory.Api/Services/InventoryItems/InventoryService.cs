using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Inventory.Api.Infrastructure;
using Inventory.Api.Infrastructure.Entities;
using Inventory.Api.ServicesModels;
using Inventory.Api.ServicesModels.InventoryServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;

namespace Inventory.Api.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly InventoryContext context;
        private readonly IStringLocalizer inventortLocalizer;

        public InventoryService(
            InventoryContext context,
            IStringLocalizer<InventoryResources> inventortLocalizer)
        {
            this.context = context;
            this.inventortLocalizer = inventortLocalizer;
        }

        public async Task<InventoryServiceModel> UpdateAsync(UpdateInventoryServiceModel serviceModel)
        {
            var inventoryProduct = await this.context.Inventory.FirstOrDefaultAsync(x => x.Id == serviceModel.Id && x.SellerId == serviceModel.OrganisationId.Value && x.IsActive);
            
            if (inventoryProduct == null)
            {
                throw new CustomException(this.inventortLocalizer.GetString("InventoryNotFound"), (int)HttpStatusCode.NotFound);
            }

            inventoryProduct.WarehouseId = serviceModel.WarehouseId.Value;
            inventoryProduct.ProductId = serviceModel.ProductId.Value;
            inventoryProduct.ProductName = serviceModel.ProductName;
            inventoryProduct.ProductSku = serviceModel.ProductSku;
            inventoryProduct.Quantity = serviceModel.Quantity;
            inventoryProduct.Ean = serviceModel.Ean;
            inventoryProduct.RestockableInDays = serviceModel.RestockableInDays;
            inventoryProduct.AvailableQuantity = serviceModel.AvailableQuantity;
            inventoryProduct.ExpectedDelivery = serviceModel.ExpectedDelivery;
            inventoryProduct.LastModifiedDate = DateTime.UtcNow;

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetInventoryServiceModel { Id = inventoryProduct.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }

        public async Task<InventoryServiceModel> CreateAsync(CreateInventoryServiceModel serviceModel)
        {
            var inventoryProduct = new InventoryItem
            {
                WarehouseId = serviceModel.WarehouseId.Value,
                ProductId = serviceModel.ProductId.Value,
                ProductName = serviceModel.ProductName,
                ProductSku = serviceModel.ProductSku,
                Quantity = serviceModel.Quantity,
                Ean = serviceModel.Ean,
                AvailableQuantity = serviceModel.AvailableQuantity,
                RestockableInDays = serviceModel.RestockableInDays,
                ExpectedDelivery = serviceModel.ExpectedDelivery,
                SellerId = serviceModel.OrganisationId.Value
            };

            this.context.Inventory.Add(inventoryProduct.FillCommonProperties());

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetInventoryServiceModel { Id = inventoryProduct.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }

        public async Task SyncInventoryProducts(UpdateProductsInventoryServiceModel model)
        {
            var inventoryProducts = this.context.Inventory
                .Join(this.context.Warehouses, inventory => inventory.WarehouseId, warehouse => warehouse.Id, (inventory, warehouse) => 
                new
                {
                    Id = inventory.Id,
                    ProductSku = inventory.ProductSku,
                    WarehouseName = warehouse.Name,
                });


            foreach (var item in model.InventoryItems.OrEmptyIfNull())
            {
                var inventory = await inventoryProducts.FirstOrDefaultAsync(x => x.ProductSku == item.ProductSku && x.WarehouseName == item.WarehouseName);

                if (inventory is not null)
                {
                    var inventoryItem = await this.context.Inventory.FirstOrDefaultAsync(x => x.Id == inventory.Id);

                    inventoryItem.ExpectedDelivery = item.ExpectedDelivery;
                    inventoryItem.RestockableInDays = item.RestockableInDays;
                    inventoryItem.AvailableQuantity = item.AvailableQuantity;
                    inventoryItem.Quantity = item.Quantity;
                    inventoryItem.LastModifiedDate = DateTime.UtcNow;
                }
                else
                {
                    var warehouse = await this.context.Warehouses.FirstOrDefaultAsync(x => x.Name == item.WarehouseName);

                    if (warehouse is not null)
                    {
                        var inventoryProduct = new InventoryItem
                        {
                            WarehouseId = warehouse.Id,
                            ProductId = item.ProductId.Value,
                            ProductName = item.ProductName,
                            ProductSku = item.ProductSku,
                            Quantity = item.Quantity,
                            AvailableQuantity = item.AvailableQuantity,
                            ExpectedDelivery = item.ExpectedDelivery,
                            SellerId = model.OrganisationId.Value
                        };

                        this.context.Inventory.Add(inventoryProduct.FillCommonProperties());
                    }
                }
            }

            await this.context.SaveChangesAsync();
        }

        public async Task<InventoryServiceModel> GetAsync(GetInventoryServiceModel model)
        {
            var inventoryProduct = from c in this.context.Inventory
                                   join warehouse in this.context.Warehouses on c.WarehouseId equals warehouse.Id
                                   where c.Id == model.Id.Value && c.IsActive
                                   select new InventoryServiceModel
                                   {
                                       Id = c.Id,
                                       ProductId = c.ProductId,
                                       ProductName = c.ProductName,
                                       ProductSku = c.ProductSku,
                                       WarehouseId = c.WarehouseId,
                                       WarehouseName = warehouse.Name,
                                       Quantity = c.Quantity,
                                       Ean = c.Ean,
                                       AvailableQuantity = c.AvailableQuantity.Value,
                                       RestockableInDays = c.RestockableInDays.Value,
                                       ExpectedDelivery = c.ExpectedDelivery.Value,
                                       LastModifiedDate = c.LastModifiedDate,
                                       CreatedDate = c.CreatedDate
                                   };

            return await inventoryProduct.FirstOrDefaultAsync();
        }

        public async Task<PagedResults<IEnumerable<InventoryServiceModel>>> GetAsync(GetInventoriesServiceModel model)
        {
            var inventories = from c in this.context.Inventory
                              join warehouse in this.context.Warehouses on c.WarehouseId equals warehouse.Id
                              where c.SellerId == model.OrganisationId.Value && c.IsActive
                              select new InventoryServiceModel
                              {
                                Id = c.Id,
                                ProductId = c.ProductId,
                                ProductName = c.ProductName,
                                ProductSku = c.ProductSku,
                                WarehouseId = c.WarehouseId,
                                WarehouseName = warehouse.Name,
                                Quantity = c.Quantity,
                                Ean = c.Ean,
                                AvailableQuantity = c.AvailableQuantity.Value,
                                RestockableInDays= c.RestockableInDays,
                                ExpectedDelivery = c.ExpectedDelivery,
                                LastModifiedDate = c.LastModifiedDate,
                                CreatedDate = c.CreatedDate
                              };

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                inventories = inventories.Where(x => x.ProductName.StartsWith(model.SearchTerm) || x.WarehouseName.StartsWith(model.SearchTerm) || x.ProductSku.StartsWith(model.SearchTerm));
            }

            inventories = inventories.ApplySort(model.OrderBy);

            return inventories.PagedIndex(new Pagination(inventories.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<PagedResults<IEnumerable<InventoryServiceModel>>> GetByIdsAsync(GetInventoriesByIdsServiceModel model)
        {
            var inventoryProducts = from c in this.context.Inventory
                             join warehouse in this.context.Warehouses on c.WarehouseId equals warehouse.Id
                             where model.Ids.Contains(c.Id) && c.SellerId == model.OrganisationId.Value && c.IsActive
                             select new InventoryServiceModel
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

            return inventoryProducts.PagedIndex(new Pagination(inventoryProducts.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<InventorySumServiceModel> GetInventoryByProductId(GetInventoryByProductIdServiceModel model)
        {
            var inventory = from i in this.context.Inventory
                            join warehouse in this.context.Warehouses on i.WarehouseId equals warehouse.Id
                            where i.ProductId == model.ProductId.Value && i.IsActive
                            select new
                            {
                                Id = i.Id,
                                ProductId = i.ProductId,
                                ProductName = i.ProductName,
                                ProductSku = i.ProductSku,
                                Quantity = i.Quantity,
                                Ean = i.Ean,
                                AvailableQuantity = i.AvailableQuantity,
                                ExpectedDelivery = i.ExpectedDelivery,
                                RestockableInDays = i.RestockableInDays,
                                WarehouseId = i.WarehouseId,
                                WarehouseName = warehouse.Name,
                                LastModifiedDate = i.LastModifiedDate,
                                CreatedDate = i.CreatedDate
                            };

            if (inventory.OrEmptyIfNull().Any())
            {
                var inventorySum = new InventorySumServiceModel
                {
                    ProductId = model.ProductId.Value,
                    ProductName = inventory.FirstOrDefault().ProductName,
                    ProductSku = inventory.FirstOrDefault().ProductSku,
                    Ean = inventory.FirstOrDefault().Ean,
                    AvailableQuantity = inventory.Sum(x => x.AvailableQuantity),
                    Quantity = inventory.Sum(x => x.Quantity),
                    ExpectedDelivery = inventory.Min(x => x.ExpectedDelivery),
                    RestockableInDays = inventory.Min(x => x.RestockableInDays),
                    Details = inventory.Select(item => new InventoryServiceModel
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        ProductSku = item.ProductSku,
                        AvailableQuantity = item.AvailableQuantity,
                        Quantity = item.Quantity,
                        Ean = item.Ean,
                        ExpectedDelivery = item.ExpectedDelivery,
                        WarehouseId = item.WarehouseId,
                        WarehouseName = item.WarehouseName,
                        RestockableInDays = item.RestockableInDays,
                        LastModifiedDate = item.LastModifiedDate,
                        CreatedDate = item.CreatedDate
                    })
                };

                return inventorySum;
            }

            return default;                
        }

        public async Task<InventorySumServiceModel> GetInventoryByProductSku(GetInventoryByProductSkuServiceModel model)
        {
            var inventory = from i in this.context.Inventory
                            join warehouse in this.context.Warehouses on i.WarehouseId equals warehouse.Id
                            where i.ProductSku == model.ProductSku && i.IsActive
                            select new
                            {
                                Id = i.Id,
                                ProductId = i.ProductId,
                                ProductName = i.ProductName,
                                ProductSku = i.ProductSku,
                                Quantity = i.Quantity,
                                Ean = i.Ean,
                                AvailableQuantity = i.AvailableQuantity,
                                ExpectedDelivery = i.ExpectedDelivery,
                                RestockableInDays = i.RestockableInDays,
                                WarehouseId = i.WarehouseId,
                                WarehouseName = warehouse.Name,
                                LastModifiedDate = i.LastModifiedDate,
                                CreatedDate = i.CreatedDate
                            };

            if (inventory.OrEmptyIfNull().Any())
            {
                var inventorySum = new InventorySumServiceModel
                {
                    ProductId = inventory.FirstOrDefault().ProductId,
                    ProductName = inventory.FirstOrDefault().ProductName,
                    ProductSku = model.ProductSku,
                    Ean = inventory.FirstOrDefault().Ean,
                    AvailableQuantity = inventory.Sum(x => x.AvailableQuantity),
                    Quantity = inventory.Sum(x => x.Quantity),
                    ExpectedDelivery = inventory.Min(x => x.ExpectedDelivery),
                    RestockableInDays = inventory.Min(x => x.RestockableInDays),
                    Details = inventory.Select(item => new InventoryServiceModel
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        ProductSku = item.ProductSku,
                        AvailableQuantity = item.AvailableQuantity,
                        Quantity = item.Quantity,
                        Ean = item.Ean,
                        ExpectedDelivery = item.ExpectedDelivery,
                        WarehouseId = item.WarehouseId,
                        WarehouseName = item.WarehouseName,
                        RestockableInDays = item.RestockableInDays,
                        LastModifiedDate = item.LastModifiedDate,
                        CreatedDate = item.CreatedDate
                    })
                };

                return inventorySum;
            }

            return default;
        }

        public async Task UpdateInventoryProduct(Guid? ProductId, string ProductName, string ProductSku, Guid? OrganisationId)
        {
            var inventoryProduct = await this.context.Inventory.FirstOrDefaultAsync(x => x.ProductId == ProductId.Value && x.SellerId == OrganisationId.Value && x.IsActive);
            if (inventoryProduct != null)
            {
                inventoryProduct.ProductName = ProductName;
                inventoryProduct.ProductSku = ProductSku;
                inventoryProduct.LastModifiedDate = DateTime.UtcNow;

                await this.context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(DeleteInventoryServiceModel model)
        {
            var inventory = await this.context.Inventory.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);
            if (inventory == null)
            {
                throw new CustomException(this.inventortLocalizer.GetString("InventoryNotFound"), (int)HttpStatusCode.NotFound);
            }
            inventory.IsActive = false;
            await this.context.SaveChangesAsync();
        }

        public async Task<PagedResults<IEnumerable<InventorySumServiceModel>>> GetAvailableProductsInventoriesAsync(GetInventoriesServiceModel model)
        {
            var inventories = (from i in this.context.Inventory
                            group i by new { i.ProductId } into gpi
                            where gpi.Sum(x => x.AvailableQuantity) > 0
                            select new InventorySumServiceModel
                            {
                                ProductId = gpi.Key.ProductId,
                                ProductName = gpi.FirstOrDefault().ProductName,
                                ProductSku = gpi.FirstOrDefault().ProductSku,
                                AvailableQuantity = gpi.Sum(x => x.AvailableQuantity),
                                Quantity = gpi.Sum(x => x.Quantity),
                                Ean = gpi.FirstOrDefault().Ean,
                                ExpectedDelivery = gpi.Min(x => x.ExpectedDelivery),
                                RestockableInDays = gpi.Min(x => x.RestockableInDays)
                            }).OrderByDescending(x => x.AvailableQuantity);

                return inventories.PagedIndex(new Pagination(inventories.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task UpdateInventoryBasket(Guid? ProductId, int BookedQuantity)
        {
            var inventoryProduct = this.context.Inventory.FirstOrDefault(x => x.ProductId == ProductId.Value && x.IsActive);
            if (inventoryProduct != null)
            {
                var productQuantity = inventoryProduct.Quantity + BookedQuantity;

                if (productQuantity < 0)
                {
                    productQuantity = 0;
                }

                inventoryProduct.Quantity = productQuantity;
                inventoryProduct.AvailableQuantity = productQuantity;
                inventoryProduct.LastModifiedDate = DateTime.UtcNow;

                await this.context.SaveChangesAsync();
            }
        }
    }
}
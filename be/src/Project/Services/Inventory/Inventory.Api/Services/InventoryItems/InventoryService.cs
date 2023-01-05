using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Inventory.Api.Infrastructure;
using Inventory.Api.Infrastructure.Entities;
using Inventory.Api.ServicesModels.InventoryServiceModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;

namespace Inventory.Api.Services.InventoryItems
{
    public class InventoryService : IInventoryService
    {
        private readonly InventoryContext _context;
        private readonly IStringLocalizer _inventoryLocalizer;

        public InventoryService(
            InventoryContext context,
            IStringLocalizer<InventoryResources> inventoryLocalizer)
        {
            _context = context;
            _inventoryLocalizer = inventoryLocalizer;
        }

        public async Task<InventoryServiceModel> UpdateAsync(UpdateInventoryServiceModel serviceModel)
        {
            var inventory = await _context.Inventory.FirstOrDefaultAsync(x => x.Id == serviceModel.Id && x.SellerId == serviceModel.OrganisationId.Value && x.IsActive);
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == inventory.ProductId && x.IsActive);

            if (product is null || inventory is null)
            {
                throw new CustomException(_inventoryLocalizer.GetString("InventoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            product.Name = serviceModel.ProductName;
            product.Sku = serviceModel.ProductSku;
            product.Ean = serviceModel.ProductEan;
            product.LastModifiedDate = DateTime.UtcNow;

            inventory.ProductId = product.Id;
            inventory.WarehouseId = serviceModel.WarehouseId.Value;
            inventory.Quantity = serviceModel.Quantity;
            inventory.RestockableInDays = serviceModel.RestockableInDays;
            inventory.AvailableQuantity = serviceModel.AvailableQuantity;
            inventory.ExpectedDelivery = serviceModel.ExpectedDelivery;
            inventory.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await this.GetAsync(new GetInventoryServiceModel { Id = inventory.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }

        public async Task<InventoryServiceModel> CreateAsync(CreateInventoryServiceModel serviceModel)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == serviceModel.ProductId && x.IsActive);

            if (product is null)
            {
                product = new Product
                {
                    Id = serviceModel.ProductId.Value,
                    Ean = serviceModel.ProductEan,
                    Sku = serviceModel.ProductSku,
                    Name = serviceModel.ProductName
                };

                _context.Products.Add(product.FillCommonProperties());
            }

            var inventory = new InventoryItem
            {
                WarehouseId = serviceModel.WarehouseId.Value,
                ProductId = product.Id,
                Quantity = serviceModel.Quantity,
                AvailableQuantity = serviceModel.AvailableQuantity,
                RestockableInDays = serviceModel.RestockableInDays,
                ExpectedDelivery = serviceModel.ExpectedDelivery,
                SellerId = serviceModel.OrganisationId.Value
            };

            _context.Inventory.Add(inventory.FillCommonProperties());

            await _context.SaveChangesAsync();

            return await this.GetAsync(new GetInventoryServiceModel { Id = inventory.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }

        public async Task SyncProductsInventories(UpdateProductsInventoryServiceModel model)
        {
            foreach (var item in model.InventoryItems.OrEmptyIfNull())
            {
                var inventoryProduct = await _context.Inventory.FirstOrDefaultAsync(x => x.ProductId == item.ProductId && x.WarehouseId == item.WarehouseId && x.IsActive);

                if (inventoryProduct is not null)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == inventoryProduct.ProductId && x.IsActive);

                    if (product is not null)
                    {
                        product.Ean = item.ProductEan;
                        product.Sku = item.ProductSku;
                        product.Name = item.ProductName;
                        product.LastModifiedDate = DateTime.UtcNow;
                    }

                    inventoryProduct.Quantity = item.Quantity;
                    inventoryProduct.AvailableQuantity = item.AvailableQuantity;
                    inventoryProduct.ExpectedDelivery = item.ExpectedDelivery;
                    inventoryProduct.LastModifiedDate = DateTime.UtcNow;
                }
                else
                {
                    var warehouse = await _context.Warehouses.FirstOrDefaultAsync(x => x.Id == item.WarehouseId);

                    if (warehouse is not null)
                    {
                        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId && x.IsActive);

                        if (product is null)
                        {
                            product = new Product
                            {
                                Id = item.ProductId.Value,
                                Name = item.ProductName,
                                Sku = item.ProductSku,
                                Ean = item.ProductEan
                            };

                            _context.Products.Add(product.FillCommonProperties());
                        }

                        var inventoryItem = new InventoryItem
                        {
                            WarehouseId = warehouse.Id,
                            ProductId = product.Id,
                            Quantity = item.Quantity,
                            AvailableQuantity = item.AvailableQuantity,
                            ExpectedDelivery = item.ExpectedDelivery,
                            SellerId = model.OrganisationId.Value
                        };

                        _context.Inventory.Add(inventoryItem.FillCommonProperties());
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<InventoryServiceModel> GetAsync(GetInventoryServiceModel model)
        {
            var inventoryProduct = from c in _context.Inventory
                                   join warehouse in _context.Warehouses on c.WarehouseId equals warehouse.Id
                                   join product in _context.Products on c.ProductId equals product.Id
                                   where c.Id == model.Id.Value && product.IsActive && c.IsActive
                                   select new InventoryServiceModel
                                   {
                                       Id = c.Id,
                                       ProductId = c.ProductId,
                                       ProductName = product.Name,
                                       Sku = product.Sku,
                                       WarehouseId = c.WarehouseId,
                                       WarehouseName = warehouse.Name,
                                       Quantity = c.Quantity,
                                       Ean = product.Ean,
                                       AvailableQuantity = c.AvailableQuantity,
                                       RestockableInDays = c.RestockableInDays.Value,
                                       ExpectedDelivery = c.ExpectedDelivery.Value,
                                       LastModifiedDate = c.LastModifiedDate,
                                       CreatedDate = c.CreatedDate
                                   };

            return await inventoryProduct.FirstOrDefaultAsync();
        }

        public async Task<PagedResults<IEnumerable<InventoryServiceModel>>> GetAsync(GetInventoriesServiceModel model)
        {
            var inventories = from c in _context.Inventory
                              join warehouse in _context.Warehouses on c.WarehouseId equals warehouse.Id
                              join product in _context.Products on c.ProductId equals product.Id
                              where c.SellerId == model.OrganisationId.Value && product.IsActive && c.IsActive
                              select new InventoryServiceModel
                              {
                                Id = c.Id,
                                ProductId = c.ProductId,
                                ProductName = product.Name,
                                Sku = product.Sku,
                                WarehouseId = c.WarehouseId,
                                WarehouseName = warehouse.Name,
                                Quantity = c.Quantity,
                                Ean = product.Ean,
                                AvailableQuantity = c.AvailableQuantity,
                                RestockableInDays= c.RestockableInDays,
                                ExpectedDelivery = c.ExpectedDelivery,
                                LastModifiedDate = c.LastModifiedDate,
                                CreatedDate = c.CreatedDate
                              };

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                inventories = inventories.Where(x => x.ProductName.StartsWith(model.SearchTerm) || x.WarehouseName.StartsWith(model.SearchTerm) || x.Sku.StartsWith(model.SearchTerm));
            }

            inventories = inventories.ApplySort(model.OrderBy);

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                inventories = inventories.Take(Constants.MaxItemsPerPageLimit);

                return inventories.PagedIndex(new Pagination(inventories.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }

            return inventories.PagedIndex(new Pagination(inventories.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
        }

        public async Task<PagedResults<IEnumerable<InventoryServiceModel>>> GetByIdsAsync(GetInventoriesByIdsServiceModel model)
        {
            var inventoryProducts = from c in _context.Inventory
                             join warehouse in _context.Warehouses on c.WarehouseId equals warehouse.Id
                             join product in _context.Products on c.ProductId equals product.Id
                             where model.Ids.Contains(c.Id) && c.SellerId == model.OrganisationId.Value && product.IsActive && c.IsActive
                             select new InventoryServiceModel
                             {
                                 Id = c.Id,
                                 ProductId = c.ProductId,
                                 ProductName = product.Name,
                                 Sku = product.Sku,
                                 Ean = product.Ean,
                                 AvailableQuantity = c.AvailableQuantity,
                                 Quantity = c.Quantity,
                                 ExpectedDelivery = c.ExpectedDelivery,
                                 RestockableInDays = c.RestockableInDays,
                                 WarehouseId = c.WarehouseId,
                                 WarehouseName = warehouse.Name,
                                 LastModifiedDate = c.LastModifiedDate,
                                 CreatedDate = c.CreatedDate
                             };

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                inventoryProducts = inventoryProducts.Take(Constants.MaxItemsPerPageLimit);

                return inventoryProducts.PagedIndex(new Pagination(inventoryProducts.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }

            return inventoryProducts.PagedIndex(new Pagination(inventoryProducts.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
        }

        public async Task<InventorySumServiceModel> GetInventoryByProductId(GetInventoryByProductIdServiceModel model)
        {
            var inventory = from i in _context.Inventory
                            join warehouse in _context.Warehouses on i.WarehouseId equals warehouse.Id
                            join product in _context.Products on i.ProductId equals product.Id
                            where i.ProductId == model.ProductId.Value && product.IsActive && i.IsActive
                            select new
                            {
                                Id = i.Id,
                                ProductId = i.ProductId,
                                ProductName = product.Name,
                                ProductSku = product.Sku,
                                Quantity = i.Quantity,
                                Ean = product.Ean,
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
                    ProductEan = inventory.FirstOrDefault().Ean,
                    AvailableQuantity = inventory.Sum(x => x.AvailableQuantity),
                    Quantity = inventory.Sum(x => x.Quantity),
                    ExpectedDelivery = inventory.Min(x => x.ExpectedDelivery),
                    RestockableInDays = inventory.Min(x => x.RestockableInDays),
                    Details = inventory.Select(item => new InventoryServiceModel
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Sku = item.ProductSku,
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
            var inventory = from i in _context.Inventory
                            join warehouse in _context.Warehouses on i.WarehouseId equals warehouse.Id
                            join product in _context.Products on i.ProductId equals product.Id
                            where product.Sku == model.ProductSku && product.IsActive && i.IsActive
                            select new
                            {
                                Id = i.Id,
                                ProductId = i.ProductId,
                                ProductName = product.Name,
                                ProductSku = product.Sku,
                                Quantity = i.Quantity,
                                Ean = product.Ean,
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
                    ProductEan = inventory.FirstOrDefault().Ean,
                    AvailableQuantity = inventory.Sum(x => x.AvailableQuantity),
                    Quantity = inventory.Sum(x => x.Quantity),
                    ExpectedDelivery = inventory.Min(x => x.ExpectedDelivery),
                    RestockableInDays = inventory.Min(x => x.RestockableInDays),
                    Details = inventory.Select(item => new InventoryServiceModel
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Sku = item.ProductSku,
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

        public async Task DeleteAsync(DeleteInventoryServiceModel model)
        {
            var inventory = await _context.Inventory.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (inventory is null)
            {
                throw new CustomException(_inventoryLocalizer.GetString("InventoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            inventory.IsActive = false;

            await _context.SaveChangesAsync();
        }

        public async Task<PagedResults<IEnumerable<InventorySumServiceModel>>> GetAvailableProductsInventoriesAsync(GetInventoriesServiceModel model)
        {
            var inventories = (from i in _context.Inventory
                               join product in _context.Products on i.ProductId equals product.Id
                               where product.IsActive && i.IsActive
                               group i by new { product.Id } into gpi
                               select new InventorySumServiceModel
                               {
                                    ProductId = gpi.Key.Id,
                                    ProductName = _context.Products.FirstOrDefault(x => x.Id == gpi.FirstOrDefault().ProductId && x.IsActive).Name,
                                    ProductSku = _context.Products.FirstOrDefault(x => x.Id == gpi.FirstOrDefault().ProductId && x.IsActive).Sku,
                                    ProductEan = _context.Products.FirstOrDefault(x => x.Id == gpi.FirstOrDefault().ProductId && x.IsActive).Ean,
                                    AvailableQuantity = gpi.Sum(x => x.AvailableQuantity),
                                    Quantity = gpi.Sum(x => x.Quantity),
                                    ExpectedDelivery = gpi.Min(x => x.ExpectedDelivery),
                                    RestockableInDays = gpi.Min(x => x.RestockableInDays)
                               });

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                inventories = inventories.Take(Constants.MaxItemsPerPageLimit).OrderByDescending(x => x.AvailableQuantity);
            }

            return inventories.OrderByDescending(x => x.AvailableQuantity).PagedIndex(new Pagination(inventories.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
        }

        public async Task UpdateInventoryQuantity(Guid? productId, double bookedQuantity)
        {
            var inventory = _context.Inventory.FirstOrDefault(x => x.ProductId == productId.Value && x.IsActive);

            if (inventory is not null)
            {
                var productQuantity = inventory.Quantity + bookedQuantity;

                if (productQuantity < 0)
                {
                    productQuantity = 0;
                }

                inventory.Quantity = productQuantity;
                inventory.AvailableQuantity = productQuantity;
                inventory.LastModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }
    }
}
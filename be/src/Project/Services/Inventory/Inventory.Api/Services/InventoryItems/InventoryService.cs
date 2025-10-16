using Foundation.EventBus.Abstractions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Inventory.Api.Infrastructure;
using Inventory.Api.Infrastructure.Entities;
using Inventory.Api.IntegrationEvents;
using Inventory.Api.IntegrationEventsModels;
using Inventory.Api.ServicesModels.InventoryServiceModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
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
        private readonly IEventBus _eventBus;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(
            InventoryContext context,
            IStringLocalizer<InventoryResources> inventoryLocalizer,
            IEventBus eventBus,
            ILogger<InventoryService> logger)
        {
            _context = context;
            _inventoryLocalizer = inventoryLocalizer;
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task<InventoryServiceModel> UpdateAsync(UpdateInventoryServiceModel serviceModel)
        {
            var inventory = await _context.Inventory.FirstOrDefaultAsync(x => x.Id == serviceModel.Id && x.SellerId == serviceModel.OrganisationId.Value && x.IsActive);

            if (inventory is null)
            {
                throw new NotFoundException(_inventoryLocalizer.GetString("InventoryNotFound"));
            }

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == inventory.ProductId && x.IsActive);

            if (product is null)
            {
                throw new NotFoundException(_inventoryLocalizer.GetString("InventoryNotFound"));
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

            var productAvailableQuantityUpdateMessage = new InventoryProductsAvailableQuantityUpdateIntegrationEvent
            {
                Products = new List<ProductAvailableQuantityUpdateEventModel>
                {
                    new ProductAvailableQuantityUpdateEventModel
                    {
                        ProductSku = product.Sku,
                        AvailableQuantity = serviceModel.AvailableQuantity
                    }
                }
            };

            _eventBus.Publish(productAvailableQuantityUpdateMessage);

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

            if (serviceModel.AvailableQuantity > 0)
            {
                var productAvailableQuantityUpdateMessage = new InventoryProductsAvailableQuantityUpdateIntegrationEvent
                {
                    Products = new List<ProductAvailableQuantityUpdateEventModel>
                    {
                        new ProductAvailableQuantityUpdateEventModel
                        {
                            ProductSku = product.Sku,
                            AvailableQuantity = serviceModel.AvailableQuantity
                        }
                    }
                };

                _eventBus.Publish(productAvailableQuantityUpdateMessage);
            }

            return await this.GetAsync(new GetInventoryServiceModel { Id = inventory.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }

        public async Task SyncProductsInventories(UpdateProductsInventoryServiceModel model)
        {
            _logger.LogWarning($"Received inventory products for organisation {model.OrganisationId} with {model.InventoryItems.Count()} items");

            var productsAvailableQuantityUpdates = new List<ProductAvailableQuantityUpdateEventModel>();

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

                    productsAvailableQuantityUpdates.Add(new ProductAvailableQuantityUpdateEventModel
                    {
                        ProductSku = product.Sku,
                        AvailableQuantity = item.AvailableQuantity
                    });
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

                        productsAvailableQuantityUpdates.Add(new ProductAvailableQuantityUpdateEventModel
                        {
                            ProductSku = product.Sku,
                            AvailableQuantity = item.AvailableQuantity
                        });
                    }
                }

                await _context.SaveChangesAsync();
            }

            if (productsAvailableQuantityUpdates.Any())
            {
                var productAvailableQuantityUpdateMessage = new InventoryProductsAvailableQuantityUpdateIntegrationEvent
                {
                    Products = productsAvailableQuantityUpdates
                };

                _eventBus.Publish(productAvailableQuantityUpdateMessage);
            }
        }

        public async Task<InventoryServiceModel> GetAsync(GetInventoryServiceModel model)
        {
            var inventoryProduct = await _context.Inventory
                .Include(x => x.Product)
                .Include(x => x.Warehouse)
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (inventoryProduct is null)
            {
                throw new NotFoundException(_inventoryLocalizer.GetString("InventoryNotFound"));
            }

            return new InventoryServiceModel
            {
                Id = inventoryProduct.Id,
                ProductId = inventoryProduct.ProductId,
                ProductName = inventoryProduct.Product?.Name,
                Sku = inventoryProduct.Product?.Sku,
                WarehouseId = inventoryProduct.WarehouseId,
                WarehouseName = inventoryProduct.Warehouse?.Name,
                Quantity = inventoryProduct.Quantity,
                Ean = inventoryProduct.Product?.Ean,
                AvailableQuantity = inventoryProduct.AvailableQuantity,
                RestockableInDays = inventoryProduct.RestockableInDays,
                ExpectedDelivery = inventoryProduct.ExpectedDelivery,
                LastModifiedDate = inventoryProduct.LastModifiedDate,
                CreatedDate = inventoryProduct.CreatedDate
            };
        }

        public PagedResults<IEnumerable<InventoryServiceModel>> Get(GetInventoriesServiceModel model)
        {
            var inventoryItems = _context.Inventory.Where(x => x.SellerId == model.OrganisationId && x.IsActive)
                    .Include(x => x.Warehouse)
                    .Include(x => x.Product)
                    .AsSingleQuery()
                    .Select(x => new InventoryServiceModel
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        ProductName = x.Product.Name,
                        Sku = x.Product.Sku,
                        Ean = x.Product.Ean,
                        AvailableQuantity = x.AvailableQuantity,
                        Quantity = x.Quantity,
                        ExpectedDelivery = x.ExpectedDelivery,
                        RestockableInDays = x.RestockableInDays,
                        WarehouseId = x.WarehouseId,
                        WarehouseName = x.Warehouse.Name,
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedDate = x.CreatedDate
                    });

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                inventoryItems = inventoryItems.Where(x => x.ProductName.StartsWith(model.SearchTerm) || x.WarehouseName.StartsWith(model.SearchTerm) || x.Sku.StartsWith(model.SearchTerm));
            }

            inventoryItems = inventoryItems.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<InventoryServiceModel>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                inventoryItems = inventoryItems.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = inventoryItems.PagedIndex(new Pagination(inventoryItems.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = inventoryItems.PagedIndex(new Pagination(inventoryItems.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return pagedResults;
        }

        public PagedResults<IEnumerable<InventoryServiceModel>> GetByIds(GetInventoriesByIdsServiceModel model)
        {
            var inventoryProducts = _context.Inventory.Where(x => model.Ids.Contains(x.Id) && x.SellerId == model.OrganisationId && x.IsActive)
                    .Include(x => x.Warehouse)
                    .Include(x => x.Product)
                    .AsSingleQuery();

            inventoryProducts = inventoryProducts.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<InventoryItem>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                inventoryProducts = inventoryProducts.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = inventoryProducts.PagedIndex(new Pagination(inventoryProducts.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            } else
            {
                pagedResults = inventoryProducts.PagedIndex(new Pagination(inventoryProducts.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return new PagedResults<IEnumerable<InventoryServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new InventoryServiceModel
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product?.Name,
                    Sku = x.Product?.Sku,
                    Ean = x.Product?.Ean,
                    AvailableQuantity = x.AvailableQuantity,
                    Quantity = x.Quantity,
                    ExpectedDelivery = x.ExpectedDelivery,
                    RestockableInDays = x.RestockableInDays,
                    WarehouseId = x.WarehouseId,
                    WarehouseName = x.Warehouse?.Name,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
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
                throw new NotFoundException(_inventoryLocalizer.GetString("InventoryNotFound"));
            }

            inventory.IsActive = false;

            await _context.SaveChangesAsync();
        }

        public PagedResults<IEnumerable<InventorySumServiceModel>> GetAvailableProductsInventories(GetInventoriesServiceModel model)
        {
            var inventoryItems = _context.Inventory.Where(x => x.IsActive)
                    .Include(x => x.Product)
                    .AsSingleQuery()
                    .Select(y => new InventorySumServiceModel
                    {
                        ProductId = y.ProductId,
                        ProductEan = y.Product.Ean,
                        ProductName = y.Product.Name,
                        ProductSku = y.Product.Sku,
                        AvailableQuantity = y.AvailableQuantity,
                        Quantity = y.Quantity,
                        ExpectedDelivery = y.ExpectedDelivery,
                        RestockableInDays = y.RestockableInDays
                    });

            PagedResults<IEnumerable<InventorySumServiceModel>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                inventoryItems = inventoryItems.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = inventoryItems.PagedIndex(new Pagination(inventoryItems.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = inventoryItems.PagedIndex(new Pagination(inventoryItems.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            var groupedResults = pagedResults.Data.GroupBy(x => x.ProductId);

            return new PagedResults<IEnumerable<InventorySumServiceModel>>(inventoryItems.Count(), pagedResults.PageSize)
            {
                Data = groupedResults.OrEmptyIfNull().Select(x => new InventorySumServiceModel
                {
                    ProductId = x.Key,
                    ProductName = x.FirstOrDefault().ProductName,
                    ProductEan = x.FirstOrDefault().ProductEan,
                    ProductSku = x.FirstOrDefault().ProductSku,
                    AvailableQuantity = x.Sum(y => y.AvailableQuantity),
                    Quantity = x.Sum(y => y.Quantity),
                    ExpectedDelivery = x.Min(y => y.ExpectedDelivery),
                    RestockableInDays = x.Min(y => y.RestockableInDays)
                })
            };
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

                var productAvailableQuantityUpdateMessage = new InventoryProductsAvailableQuantityUpdateIntegrationEvent
                {
                    Products = new List<ProductAvailableQuantityUpdateEventModel>
                    {
                        new ProductAvailableQuantityUpdateEventModel
                        {
                            ProductSku = inventory.Product?.Sku,
                            AvailableQuantity = productQuantity
                        }
                    }
                };

                _eventBus.Publish(productAvailableQuantityUpdateMessage);
            }
        }

        public IEnumerable<InventorySumServiceModel> GetInventoriesByProductsIds(GetInventoriesByProductsIdsServiceModel model)
        {
            return from i in _context.Inventory
                   join warehouse in _context.Warehouses on i.WarehouseId equals warehouse.Id
                   join product in _context.Products on i.ProductId equals product.Id
                   where model.Ids.Contains(i.ProductId) && product.IsActive && i.IsActive
                   select new InventorySumServiceModel
                   {
                       ProductId = product.Id,
                       ProductName = product.Name,
                       ProductEan = product.Ean,
                       Quantity = i.Quantity,
                       AvailableQuantity = i.AvailableQuantity,
                       RestockableInDays = i.RestockableInDays,
                       ExpectedDelivery = i.ExpectedDelivery,
                   };
        }
    }
}
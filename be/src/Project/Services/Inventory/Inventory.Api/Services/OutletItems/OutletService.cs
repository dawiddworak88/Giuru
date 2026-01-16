using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Inventory.Api.Infrastructure;
using Inventory.Api.Infrastructure.Entities;
using Inventory.Api.ServicesModels.OutletServiceModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Inventory.Api.Services.OutletItems
{
    public class OutletService : IOutletService
    {
        private readonly InventoryContext _context;
        private readonly IStringLocalizer _inventoryLocalizer;
        private readonly ILogger<OutletService> _logger;

        public OutletService(
            InventoryContext context,
            IStringLocalizer<InventoryResources> inventoryLocalizer,
            ILogger<OutletService> logger)
        {
            _context = context;
            _inventoryLocalizer = inventoryLocalizer;
            _logger = logger;
        }

        public async Task<OutletServiceModel> UpdateAsync(UpdateOutletServiceModel model)
        {
            var outlet = await _context.Outlet.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (outlet is null)
            {
                throw new NotFoundException(_inventoryLocalizer.GetString("OutletNotFound"));
            }

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == outlet.ProductId && x.IsActive);

            if (product is null)
            {
                throw new NotFoundException(_inventoryLocalizer.GetString("OutletNotFound"));
            }

            var outletProductExists = _context.Outlet.Where(x => x.ProductId == product.Id && x.Id != outlet.Id && x.WarehouseId == model.WarehouseId && x.IsActive);

            if (outletProductExists.Any())
            {
                throw new ConflictException(_inventoryLocalizer.GetString("InventoryOutletSaveConflict"));
            }

            product.Name = model.ProductName;
            product.Sku = model.ProductSku;
            product.Ean = model.ProductEan;
            product.LastModifiedDate = DateTime.UtcNow;

            outlet.WarehouseId = model.WarehouseId.Value;
            outlet.ProductId = model.ProductId.Value;
            outlet.Quantity = model.Quantity;
            outlet.AvailableQuantity = model.AvailableQuantity;
            outlet.LastModifiedDate = DateTime.UtcNow;

            var outletProductTranslation = await _context.OutletTranslations.FirstOrDefaultAsync(x => x.OutletItemId == model.Id && x.Language == model.Language && x.IsActive);

            if (outletProductTranslation is not null)
            {
                outletProductTranslation.Title = model.Title;
                outletProductTranslation.Description = model.Description;
                outletProductTranslation.LastModifiedDate = DateTime.UtcNow;
            }
            else
            {
                var newOutletProductTranslation = new OutletItemTranslation
                {
                    Title = model.Title,
                    Description = model.Description,
                    Language = model.Language,
                    OutletItemId = outlet.Id
                };

                await _context.OutletTranslations.AddAsync(newOutletProductTranslation.FillCommonProperties());
            }

            await _context.SaveChangesAsync();

            return await this.GetAsync(new GetOutletServiceModel { Id = outlet.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }

        public async Task<OutletServiceModel> CreateAsync(CreateOutletServiceModel model)
        {
            var outletProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == model.ProductId && x.IsActive);
            
            if (outletProduct is null)
            {
                outletProduct = new Product
                {
                    Id = model.ProductId.Value,
                    Ean = model.ProductEan,
                    Sku = model.ProductSku,
                    Name = model.ProductName
                };

                _context.Products.Add(outletProduct.FillCommonProperties());
            }

            var outletProductExists = _context.Outlet.Where(x => x.ProductId == outletProduct.Id && x.WarehouseId == model.WarehouseId && x.IsActive);

            if (outletProductExists.Any())
            {
                throw new ConflictException(_inventoryLocalizer.GetString("InventoryOutletSaveConflict"));
            }

            var outletItem = new OutletItem
            {
                WarehouseId = model.WarehouseId.Value,
                ProductId = model.ProductId.Value,
                Quantity = model.Quantity,
                AvailableQuantity = model.AvailableQuantity,
                SellerId = model.OrganisationId.Value
            };

            _context.Outlet.Add(outletItem.FillCommonProperties());

            var outletItemTranslation = new OutletItemTranslation
            {
                Title = model.Title,
                Description = model.Description,
                Language = model.Language,
                OutletItemId = outletItem.Id
            };

            _context.OutletTranslations.Add(outletItemTranslation.FillCommonProperties());

            await _context.SaveChangesAsync();

            return await this.GetAsync(new GetOutletServiceModel { Id = outletItem.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }

        public async Task SyncProductsOutlet(UpdateOutletProductsServiceModel model)
        {
            _logger.LogWarning($"Received outlet products for organisation {model.OrganisationId} with {model.OutletItems.Count()} items");

            foreach (var item in model.OutletItems.OrEmptyIfNull())
            {
                var outletProduct = await _context.Outlet.FirstOrDefaultAsync(x => x.ProductId == item.ProductId && x.WarehouseId == item.WarehouseId && x.IsActive);

                if (outletProduct is not null)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == outletProduct.ProductId && x.IsActive);

                    if (product is not null)
                    {
                        product.Ean = item.ProductEan;
                        product.Sku = item.ProductSku;
                        product.Name = item.ProductName;
                        product.LastModifiedDate = DateTime.UtcNow;
                    }

                    var outletItemTranslation = await _context.OutletTranslations.FirstOrDefaultAsync(x => x.OutletItemId == outletProduct.Id && x.Language == model.Language && x.IsActive);

                    if (outletItemTranslation is not null)
                    {
                        outletItemTranslation.Title = item.Title;
                        outletItemTranslation.Description = item.Description;
                        outletItemTranslation.LastModifiedDate = DateTime.UtcNow;
                    }
                    else
                    {
                        outletItemTranslation = new OutletItemTranslation
                        {
                            OutletItemId = outletProduct.Id,
                            Title = item.Title,
                            Description = item.Description,
                            Language = model.Language
                        };

                        _context.OutletTranslations.Add(outletItemTranslation.FillCommonProperties());
                    }

                    outletProduct.Quantity = item.Quantity;
                    outletProduct.AvailableQuantity = item.AvailableQuantity;
                    outletProduct.LastModifiedDate = DateTime.UtcNow;
                }
                else
                {
                    var warehouse = await _context.Warehouses.FirstOrDefaultAsync(x => x.Id == item.WarehouseId && x.IsActive);

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

                        var outletItem = new OutletItem
                        {
                            WarehouseId = warehouse.Id,
                            ProductId = product.Id,
                            Quantity = item.Quantity,
                            AvailableQuantity = item.AvailableQuantity,
                            SellerId = model.OrganisationId.Value
                        };

                        _context.Outlet.Add(outletItem.FillCommonProperties());

                        var outletItemTranslation = new OutletItemTranslation
                        {
                            OutletItemId = outletItem.Id,
                            Title = item.Title,
                            Description = item.Description,
                            Language = model.Language
                        };

                        _context.OutletTranslations.Add(outletItemTranslation.FillCommonProperties());
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<OutletServiceModel> GetAsync(GetOutletServiceModel model)
        {
            var outletItem = await _context.Outlet
                    .Include(x => x.Translations)
                    .Include(x => x.Product)
                    .Include(x => x.Warehouse)
                    .AsSingleQuery()
                    .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (outletItem is null)
            {
                throw new NotFoundException(_inventoryLocalizer.GetString("OutletNotFound"));
            }

            return new OutletServiceModel
            {
                Id = outletItem.Id,
                ProductId = outletItem.ProductId,
                ProductName = outletItem.Product?.Name,
                ProductSku = outletItem.Product?.Sku,
                ProductEan = outletItem.Product?.Ean,
                Title = outletItem.Translations.FirstOrDefault(t => t.OutletItemId == outletItem.Id && t.Language == model.Language)?.Title ?? outletItem.Translations.FirstOrDefault(t => t.OutletItemId == outletItem.Id)?.Title,
                Description = outletItem.Translations.FirstOrDefault(t => t.OutletItemId == outletItem.Id && t.Language == model.Language)?.Description ?? outletItem.Translations.FirstOrDefault(t => t.OutletItemId == outletItem.Id)?.Description,
                Quantity = outletItem.Quantity,
                WarehouseId = outletItem.WarehouseId,
                WarehouseName = outletItem.Warehouse?.Name,
                AvailableQuantity = outletItem.AvailableQuantity,
                LastModifiedDate = outletItem.LastModifiedDate,
                CreatedDate = outletItem.CreatedDate
            };
        }

        public PagedResults<IEnumerable<OutletServiceModel>> Get(GetOutletsServiceModel model)
        {
            var outletItems = _context.Outlet.Where(x => x.SellerId == model.OrganisationId && x.IsActive)
                    .Include(x => x.Warehouse)
                    .Include(x => x.Product)
                    .Include(x => x.Translations)
                    .AsSingleQuery()
                    .Select(x => new OutletServiceModel
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        ProductName = x.Product.Name,
                        ProductSku = x.Product.Sku,
                        ProductEan = x.Product.Ean,
                        Title = x.Translations.FirstOrDefault(t => t.OutletItemId == x.Id && t.Language == model.Language) != null ? x.Translations.FirstOrDefault(t => t.OutletItemId == x.Id && t.Language == model.Language).Title : x.Translations.FirstOrDefault(t => t.OutletItemId == x.Id).Title,
                        Description = x.Translations.FirstOrDefault(t => t.OutletItemId == x.Id && t.Language == model.Language) != null ? x.Translations.FirstOrDefault(t => t.OutletItemId == x.Id && t.Language == model.Language).Description : x.Translations.FirstOrDefault(t => t.OutletItemId == x.Id).Description,
                        Quantity = x.Quantity,
                        WarehouseId = x.WarehouseId,
                        WarehouseName = x.Warehouse.Name,
                        AvailableQuantity = x.AvailableQuantity,
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedDate = x.CreatedDate
                    });;

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                outletItems = outletItems.Where(x => x.Title.StartsWith(model.SearchTerm) || x.Description.StartsWith(model.SearchTerm) || x.ProductSku.StartsWith(model.SearchTerm) || x.ProductName.StartsWith(model.SearchTerm) || x.ProductEan.StartsWith(model.SearchTerm));
            }

            outletItems = outletItems.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<OutletServiceModel>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                outletItems = outletItems.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = outletItems.PagedIndex(new Pagination(outletItems.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = outletItems.PagedIndex(new Pagination(outletItems.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return pagedResults;
        }

        public PagedResults<IEnumerable<OutletServiceModel>> GetByIds(GetOutletsByIdsServiceModel model)
        {
            var outletItems = _context.Outlet.Where(x => model.Ids.Contains(x.Id) && x.SellerId == model.OrganisationId && x.IsActive)
                    .Include(x => x.Warehouse)
                    .Include(x => x.Product)
                    .Include(x => x.Translations)
                    .AsSingleQuery();

            outletItems = outletItems.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<OutletItem>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                outletItems = outletItems.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = outletItems.PagedIndex(new Pagination(outletItems.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = outletItems.PagedIndex(new Pagination(outletItems.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return new PagedResults<IEnumerable<OutletServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new OutletServiceModel
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product?.Name,
                    ProductSku = x.Product?.Sku,
                    ProductEan = x.Product?.Ean,
                    Title = x.Translations.FirstOrDefault(t => t.OutletItemId == x.Id && t.Language == model.Language)?.Title ?? x.Translations.FirstOrDefault(t => t.OutletItemId == x.Id)?.Title,
                    Description = x.Translations.FirstOrDefault(t => t.OutletItemId == x.Id && t.Language == model.Language)?.Description ?? x.Translations.FirstOrDefault(t => t.OutletItemId == x.Id)?.Description,
                    Quantity = x.Quantity,
                    WarehouseId = x.WarehouseId,
                    WarehouseName = x.Warehouse?.Name,
                    AvailableQuantity = x.AvailableQuantity,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }

        public async Task<OutletSumServiceModel> GetOutletByProductId(GetOutletByProductIdServiceModel model)
        {
            var outletItems = from o in _context.Outlet
                              join product in _context.Products on o.ProductId equals product.Id
                              join warehouse in _context.Warehouses on o.WarehouseId equals warehouse.Id
                              join ot in _context.OutletTranslations on o.Id equals ot.OutletItemId
                              where o.ProductId == model.ProductId.Value && product.IsActive && o.IsActive
                              select new
                              {
                                  Id = o.Id,
                                  ProductId = o.ProductId,
                                  ProductName = product.Name,
                                  ProductSku = product.Sku,
                                  Quantity = o.Quantity,
                                  AvailableQuantity = o.AvailableQuantity,
                                  WarehouseId = o.WarehouseId,
                                  WarehouseName = warehouse.Name,
                                  ProductEan = product.Ean,
                                  Title = ot.Title,
                                  Description = ot.Description,
                                  LastModifiedDate = o.LastModifiedDate,
                                  CreatedDate = o.CreatedDate
                              };

            if (outletItems.OrEmptyIfNull().Any())
            {
                var outletSum = new OutletSumServiceModel
                {
                    ProductId = model.ProductId.Value,
                    ProductName = outletItems.FirstOrDefault().ProductName,
                    ProductSku = outletItems.FirstOrDefault().ProductSku,
                    AvailableQuantity = outletItems.Sum(x => x.AvailableQuantity),
                    Quantity = outletItems.Sum(x => x.Quantity),
                    ProductEan = outletItems.FirstOrDefault().ProductEan,
                    Title= outletItems.FirstOrDefault().Title,
                    Description = outletItems.FirstOrDefault().Description,
                    Details = outletItems.Select(item => new OutletServiceModel
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        ProductSku = item.ProductSku,
                        AvailableQuantity = item.AvailableQuantity,
                        Quantity = item.Quantity,
                        Title = item.Title,
                        Description = item.Description,
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
            var outletItems = from o in _context.Outlet
                              join product in _context.Products on o.ProductId equals product.Id
                              join warehouse in _context.Warehouses on o.WarehouseId equals warehouse.Id
                              join ot in _context.OutletTranslations on o.Id equals ot.OutletItemId
                              where product.Sku == model.ProductSku && product.IsActive && o.IsActive
                              select new
                              {
                                  Id = o.Id,
                                  ProductId = o.ProductId,
                                  ProductName = product.Name,
                                  ProductSku = product.Sku,
                                  Quantity = o.Quantity,
                                  AvailableQuantity = o.AvailableQuantity,
                                  WarehouseId = o.WarehouseId,
                                  WarehouseName = warehouse.Name,
                                  ProductEan = product.Ean,
                                  Title = ot.Title,
                                  Description = ot.Description,
                                  LastModifiedDate = o.LastModifiedDate,
                                  CreatedDate = o.CreatedDate
                              };

            if (outletItems.OrEmptyIfNull().Any())
            {
                var outletSum = new OutletSumServiceModel
                {
                    ProductId = outletItems.FirstOrDefault().ProductId,
                    ProductName = outletItems.FirstOrDefault().ProductName,
                    ProductSku = outletItems.FirstOrDefault().ProductSku,
                    AvailableQuantity = outletItems.Sum(x => x.AvailableQuantity),
                    Quantity = outletItems.Sum(x => x.Quantity),
                    ProductEan = outletItems.FirstOrDefault().ProductEan,
                    Title = outletItems.FirstOrDefault().Title,
                    Description = outletItems.FirstOrDefault().Description,
                    Details = outletItems.Select(item => new OutletServiceModel
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        ProductSku = item.ProductSku,
                        AvailableQuantity = item.AvailableQuantity,
                        Quantity = item.Quantity,
                        Title = item.Title,
                        Description = item.Description,
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

        public async Task DeleteAsync(DeleteOutletServiceModel model)
        {
            var outlet = await _context.Outlet.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (outlet == null)
            {
                throw new NotFoundException(_inventoryLocalizer.GetString("OutletNotFound"));
            }

            outlet.IsActive = false;

            await _context.SaveChangesAsync();
        }

        public PagedResults<IEnumerable<OutletSumServiceModel>> GetAvailableProductsOutlets(GetOutletsServiceModel model)
        {
            var outletItems = _context.Outlet.Where(x => x.IsActive)
                    .Include(x => x.Product)
                    .Include(x => x.Translations)
                    .AsSingleQuery()
                    .GroupBy(x => x.ProductId)
                    .Where(x => x.Sum(y => y.AvailableQuantity) > 0)
                    .Select(y => new OutletSumServiceModel
                    { 
                        ProductId = y.FirstOrDefault().ProductId,
                        ProductName = y.FirstOrDefault().Product.Name,
                        ProductSku = y.FirstOrDefault().Product.Sku,
                        ProductEan = y.FirstOrDefault().Product.Ean,
                        AvailableQuantity = y.FirstOrDefault().AvailableQuantity,
                        Quantity = y.FirstOrDefault().Quantity,
                        OutletId = y.FirstOrDefault().Id,
                        Title = y.FirstOrDefault().Translations.FirstOrDefault(t => t.OutletItemId == y.FirstOrDefault().Id && t.Language == model.Language) != null ? y.FirstOrDefault().Translations.FirstOrDefault(t => t.OutletItemId == y.FirstOrDefault().Id && t.Language == model.Language).Title : y.FirstOrDefault().Translations.FirstOrDefault(t => t.OutletItemId == y.FirstOrDefault().Id).Title,
                        Description = y.FirstOrDefault().Translations.FirstOrDefault(t => t.OutletItemId == y.FirstOrDefault().Id && t.Language == model.Language) != null ? y.FirstOrDefault().Translations.FirstOrDefault(t => t.OutletItemId == y.FirstOrDefault().Id && t.Language == model.Language).Description : y.FirstOrDefault().Translations.FirstOrDefault(t => t.OutletItemId == y.FirstOrDefault().Id).Description
                    });

            PagedResults<IEnumerable<OutletSumServiceModel>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                outletItems = outletItems.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = outletItems.PagedIndex(new Pagination(outletItems.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = outletItems.PagedIndex(new Pagination(outletItems.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return pagedResults;
        }

        public async Task UpdateOutletQuantity(Guid? productId, double bookedQuantity)
        {
            var outlet = _context.Outlet.FirstOrDefault(x => x.ProductId == productId.Value && x.IsActive);

            if (outlet != null)
            {
                var productQuantity = outlet.Quantity + bookedQuantity;

                if (productQuantity < 0)
                {
                    productQuantity = 0;
                }

                outlet.Quantity = productQuantity;
                outlet.AvailableQuantity = productQuantity;
                outlet.LastModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }

        public IEnumerable<OutletSumServiceModel> GetOutletsByProductsIds(GetOutletsByProductsIdsServiceModel model)
        {
            return from o in _context.Outlet
                   join warehouse in _context.Warehouses on o.WarehouseId equals warehouse.Id
                   join product in _context.Products on o.ProductId equals product.Id
                   join ot in _context.OutletTranslations on o.Id equals ot.OutletItemId
                   where model.Ids.Contains(o.ProductId) && product.IsActive && o.IsActive
                   select new OutletSumServiceModel
                   {
                       ProductId = product.Id,
                       ProductName = product.Name,
                       ProductEan = product.Ean,
                       ProductSku = product.Sku,
                       Title = ot.Title,
                       Description = ot.Description,
                       Quantity = o.Quantity,
                       AvailableQuantity = o.AvailableQuantity,
                   };
        }
    }
}

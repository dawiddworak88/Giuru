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
        private readonly IStringLocalizer inventoryLocalizer;

        public OutletService(
            InventoryContext context,
            IStringLocalizer<InventoryResources> inventoryLocalizer)
        {
            this.context = context;
            this.inventoryLocalizer = inventoryLocalizer;
        }

        public async Task<OutletServiceModel> UpdateAsync(UpdateOutletServiceModel model)
        {
            var outlet = await this.context.Outlet.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);
            var product = await this.context.Products.FirstOrDefaultAsync(x => x.Id == outlet.ProductId && x.IsActive);

            if (product is null || outlet is null)
            {
                throw new CustomException(this.inventoryLocalizer.GetString("InventoryNotFound"), (int)HttpStatusCode.NotFound);
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

            var outletProductTranslation = await this.context.OutletTranslations.FirstOrDefaultAsync(x => x.OutletItemId == model.Id && x.Language == model.Language && x.IsActive);
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

                await this.context.OutletTranslations.AddAsync(newOutletProductTranslation.FillCommonProperties());
            }

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetOutletServiceModel { Id = outlet.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }

        public async Task<OutletServiceModel> CreateAsync(CreateOutletServiceModel model)
        {
            var outletProduct = await this.context.Products.FirstOrDefaultAsync(x => x.Id == model.ProductId && x.IsActive);
            
            if (outletProduct is null)
            {
                outletProduct = new Product
                {
                    Id = model.ProductId.Value,
                    Ean = model.ProductEan,
                    Sku = model.ProductSku,
                    Name = model.ProductName
                };

                this.context.Products.Add(outletProduct.FillCommonProperties());
            }

            var outletItem = new OutletItem
            {
                WarehouseId = model.WarehouseId.Value,
                ProductId = model.ProductId.Value,
                Quantity = model.Quantity,
                AvailableQuantity = model.AvailableQuantity,
                SellerId = model.OrganisationId.Value
            };

            this.context.Outlet.Add(outletItem.FillCommonProperties());

            var outletItemTranslation = new OutletItemTranslation
            {
                Title = model.Title,
                Description = model.Description,
                Language = model.Language,
                OutletItemId = outletItem.Id
            };

            this.context.OutletTranslations.Add(outletItemTranslation.FillCommonProperties());

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetOutletServiceModel { Id = outletItem.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }

        public async Task SyncProductsOutlet(UpdateOutletProductsServiceModel model)
        {
            foreach (var item in model.OutletItems.OrEmptyIfNull())
            {
                var outletProduct = await this.context.Outlet.FirstOrDefaultAsync(x => x.ProductId == item.ProductId && x.WarehouseId == item.WarehouseId && x.IsActive);

                if (outletProduct is not null)
                {
                    var product = await this.context.Products.FirstOrDefaultAsync(x => x.Id == outletProduct.ProductId && x.IsActive);

                    if (product is not null)
                    {
                        product.Ean = item.ProductEan;
                        product.Sku = item.ProductSku;
                        product.Name = item.ProductName;
                        product.LastModifiedDate = DateTime.UtcNow;
                    }

                    var outletItemTranslation = await this.context.OutletTranslations.FirstOrDefaultAsync(x => x.OutletItemId == outletProduct.Id && x.Language == model.Language && x.IsActive);

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

                        this.context.OutletTranslations.Add(outletItemTranslation.FillCommonProperties());
                    }

                    outletProduct.Quantity = item.Quantity;
                    outletProduct.AvailableQuantity = item.AvailableQuantity;
                    outletProduct.LastModifiedDate = DateTime.UtcNow;
                }
                else
                {
                    var warehouse = await this.context.Warehouses.FirstOrDefaultAsync(x => x.Id == item.WarehouseId && x.IsActive);

                    if (warehouse is not null)
                    {
                        var product = await this.context.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId && x.IsActive);

                        if (product is null)
                        {
                            product = new Product
                            {
                                Id = item.ProductId.Value,
                                Name = item.ProductName,
                                Sku = item.ProductSku,
                                Ean = item.ProductEan
                            };

                            this.context.Products.Add(product.FillCommonProperties());
                        }

                        var outletItem = new OutletItem
                        {
                            WarehouseId = warehouse.Id,
                            ProductId = product.Id,
                            Quantity = item.Quantity,
                            AvailableQuantity = item.AvailableQuantity,
                            SellerId = model.OrganisationId.Value
                        };

                        this.context.Outlet.Add(outletItem.FillCommonProperties());

                        var outletItemTranslation = new OutletItemTranslation
                        {
                            OutletItemId = outletItem.Id,
                            Title = item.Title,
                            Description = item.Description,
                            Language = model.Language
                        };

                        this.context.OutletTranslations.Add(outletItemTranslation.FillCommonProperties());
                    }
                }

                await this.context.SaveChangesAsync();
            }
        }

        public async Task<OutletServiceModel> GetAsync(GetOutletServiceModel model)
        {
            var outletItem = (from o in this.context.Outlet
                             join p in this.context.Products on o.ProductId equals p.Id
                             where o.Id == model.Id && o.IsActive
                             select new
                             {
                                 o.Id,
                                 o.ProductId,
                                 ProductName = p.Name,
                                 ProductSku = p.Sku,
                                 ProductEan = p.Ean,
                                 o.WarehouseId,
                                 o.Quantity,
                                 o.AvailableQuantity,
                                 o.LastModifiedDate,
                                 o.CreatedDate
                             }).FirstOrDefault();

            if (outletItem is not null)
            {
                var item = new OutletServiceModel
                {
                    Id = outletItem.Id,
                    ProductId = outletItem.ProductId,
                    ProductName = outletItem.ProductName,
                    ProductSku = outletItem.ProductSku,
                    WarehouseId = outletItem.WarehouseId,
                    Quantity = outletItem.Quantity,
                    ProductEan = outletItem.ProductEan,
                    AvailableQuantity = outletItem.AvailableQuantity,
                    LastModifiedDate = outletItem.LastModifiedDate,
                    CreatedDate = outletItem.CreatedDate
                };

                var warehouse = await this.context.Warehouses.FirstOrDefaultAsync(x => x.Id == outletItem.WarehouseId);
                
                if (warehouse is not null)
                {
                    item.WarehouseName = warehouse.Name;
                }

                var itemTranslation = await this.context.OutletTranslations.FirstOrDefaultAsync(x => x.OutletItemId == outletItem.Id && x.Language == model.Language && x.IsActive);

                if (itemTranslation is null)
                {
                    itemTranslation = await this.context.OutletTranslations.FirstOrDefaultAsync(x => x.OutletItemId == outletItem.Id && x.IsActive);
                }

                item.Title = itemTranslation?.Title;
                item.Description = itemTranslation?.Description;

                return item;
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<OutletServiceModel>>> GetAsync(GetOutletsServiceModel model)
        {
            var outletItems = from o in this.context.Outlet
                              join p in this.context.Products on o.ProductId equals p.Id
                              join t in this.context.OutletTranslations on o.Id equals t.OutletItemId
                              where o.IsActive
                              select new
                              {
                                  o.Id,
                                  t.Title,
                                  t.Description,
                                  o.ProductId,
                                  ProductName = p.Name,
                                  ProductSku = p.Sku,
                                  ProductEan = p.Ean,
                                  o.WarehouseId,
                                  o.Quantity,
                                  o.AvailableQuantity,
                                  o.LastModifiedDate,
                                  o.CreatedDate
                              };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                outletItems = outletItems.Where(x => x.Title.StartsWith(model.SearchTerm) || x.Description.StartsWith(model.SearchTerm));
            }

            outletItems = outletItems.ApplySort(model.OrderBy);

            var pagedResults = outletItems.PagedIndex(new Pagination(outletItems.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedItemsServiceModel = new PagedResults<IEnumerable<OutletServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var outlet = new List<OutletServiceModel>();

            foreach (var outletItem in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var item = new OutletServiceModel
                {
                    Id = outletItem.Id,
                    ProductId = outletItem.ProductId,
                    ProductName = outletItem.ProductName,
                    ProductSku = outletItem.ProductSku,
                    WarehouseId = outletItem.WarehouseId,
                    Quantity = outletItem.Quantity,
                    ProductEan = outletItem.ProductEan,
                    AvailableQuantity = outletItem.AvailableQuantity,
                    LastModifiedDate = outletItem.LastModifiedDate,
                    CreatedDate = outletItem.CreatedDate
                };

                var warehouse = await this.context.Warehouses.FirstOrDefaultAsync(x => x.Id == outletItem.WarehouseId);

                if (warehouse is not null)
                {
                    item.WarehouseName = warehouse.Name;
                }

                var itemTranslation = await this.context.OutletTranslations.FirstOrDefaultAsync(x => x.OutletItemId == outletItem.Id && x.Language == model.Language && x.IsActive);

                if (itemTranslation is null)
                {
                    itemTranslation = await this.context.OutletTranslations.FirstOrDefaultAsync(x => x.OutletItemId == outletItem.Id && x.IsActive);
                }

                item.Title = itemTranslation?.Title;
                item.Description = itemTranslation?.Description;

                outlet.Add(item);
            }

            pagedItemsServiceModel.Data = outlet;

            return pagedItemsServiceModel;
        }

        public async Task<PagedResults<IEnumerable<OutletServiceModel>>> GetByIdsAsync(GetOutletsByIdsServiceModel model)
        {
            var outletItems = from o in this.context.Outlet
                              join p in this.context.Products on o.ProductId equals p.Id
                              where model.Ids.Contains(o.Id) && o.IsActive
                              select new
                              {
                                  o.Id,
                                  o.ProductId,
                                  ProductName = p.Name,
                                  ProductSku = p.Sku,
                                  ProductEan = p.Ean,
                                  o.WarehouseId,
                                  o.Quantity,
                                  o.AvailableQuantity,
                                  o.LastModifiedDate,
                                  o.CreatedDate
                              };

            outletItems = outletItems.ApplySort(model.OrderBy);

            var pagedResults = outletItems.PagedIndex(new Pagination(outletItems.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedItemsServiceModel = new PagedResults<IEnumerable<OutletServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var outlet = new List<OutletServiceModel>();

            foreach (var outletItem in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var item = new OutletServiceModel
                {
                    Id = outletItem.Id,
                    ProductId = outletItem.ProductId,
                    ProductName = outletItem.ProductName,
                    ProductSku = outletItem.ProductSku,
                    WarehouseId = outletItem.WarehouseId,
                    Quantity = outletItem.Quantity,
                    ProductEan = outletItem.ProductEan,
                    AvailableQuantity = outletItem.AvailableQuantity,
                    LastModifiedDate = outletItem.LastModifiedDate,
                    CreatedDate = outletItem.CreatedDate
                };

                var itemTranslation = await this.context.OutletTranslations.FirstOrDefaultAsync(x => x.OutletItemId == outletItem.Id && x.Language == model.Language && x.IsActive);

                if (itemTranslation is null)
                {
                    itemTranslation = await this.context.OutletTranslations.FirstOrDefaultAsync(x => x.OutletItemId == outletItem.Id && x.IsActive);
                }

                item.Title = itemTranslation?.Title;
                item.Description = itemTranslation?.Description;

                outlet.Add(item);
            }

            pagedItemsServiceModel.Data = outlet;

            return pagedItemsServiceModel;
        }

        public async Task<OutletSumServiceModel> GetOutletByProductId(GetOutletByProductIdServiceModel model)
        {
            var outletItems = from o in this.context.Outlet
                              join product in this.context.Products on o.ProductId equals product.Id
                              join warehouse in this.context.Warehouses on o.WarehouseId equals warehouse.Id
                              join ot in this.context.OutletTranslations on o.Id equals ot.OutletItemId
                              where o.ProductId == model.ProductId.Value && o.IsActive
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
            var outletItems = from o in this.context.Outlet
                              join product in this.context.Products on o.ProductId equals product.Id
                              join warehouse in this.context.Warehouses on o.WarehouseId equals warehouse.Id
                              join ot in this.context.OutletTranslations on o.Id equals ot.OutletItemId
                              where product.Sku == model.ProductSku && o.IsActive
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
            var outlet = await this.context.Outlet.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (outlet == null)
            {
                throw new CustomException(this.inventoryLocalizer.GetString("OutletNotFound"), (int)HttpStatusCode.NotFound);
            }

            outlet.IsActive = false;

            await this.context.SaveChangesAsync();
        }

        public async Task<PagedResults<IEnumerable<OutletSumServiceModel>>> GetAvailableProductsOutletsAsync(GetOutletsServiceModel model)
        {
            var outletItems = (from o in this.context.Outlet
                               join p in this.context.Products on o.ProductId equals p.Id
                               group o by new { o.ProductId } into gpi
                               where gpi.Sum(x => x.AvailableQuantity) > 0
                               select new OutletSumServiceModel
                               {
                                   ProductId = gpi.Key.ProductId,
                                   OutletId = gpi.FirstOrDefault().Id,
                                   ProductName = this.context.Products.FirstOrDefault(x => x.Id == gpi.FirstOrDefault().ProductId && x.IsActive).Name,
                                   ProductSku = this.context.Products.FirstOrDefault(x => x.Id == gpi.FirstOrDefault().ProductId && x.IsActive).Sku,
                                   ProductEan = this.context.Products.FirstOrDefault(x => x.Id == gpi.FirstOrDefault().ProductId && x.IsActive).Ean,
                                   AvailableQuantity = gpi.Sum(x => x.AvailableQuantity),
                                   Quantity = gpi.Sum(x => x.Quantity)
                               }).OrderByDescending(x => x.AvailableQuantity);

            var pagedResults = outletItems.PagedIndex(new Pagination(outletItems.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedItemsServiceModel = new PagedResults<IEnumerable<OutletSumServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var outlet = new List<OutletSumServiceModel>();

            foreach (var outletItem in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var itemTranslation = await this.context.OutletTranslations.FirstOrDefaultAsync(x => x.OutletItemId == outletItem.OutletId && x.Language == model.Language && x.IsActive);

                if (itemTranslation is null)
                {
                    itemTranslation = await this.context.OutletTranslations.FirstOrDefaultAsync(x => x.OutletItemId == outletItem.OutletId && x.IsActive);
                }

                outletItem.Title = itemTranslation?.Title;
                outletItem.Description = itemTranslation?.Description;

                outlet.Add(outletItem);
            }

            pagedItemsServiceModel.Data = outlet;

            return pagedItemsServiceModel;
        }

        public async Task UpdateOutletQuantity(Guid? productId, double bookedQuantity)
        {
            var outlet = this.context.Outlet.FirstOrDefault(x => x.ProductId == productId.Value && x.IsActive);

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

                await this.context.SaveChangesAsync();
            }
        }
    }
}

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
using Newtonsoft.Json;
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
            outletProduct.Ean = model.Ean;
            outletProduct.LastModifiedDate = DateTime.UtcNow;

            var outletProductTranslation = await this.context.OutletTranslations.FirstOrDefaultAsync(x => x.OutletItemId == model.Id && x.Language == model.Language && x.IsActive);
            if (outletProductTranslation is not null)
            {
                outletProductTranslation.Title = model.Title;
                outletProductTranslation.Description = model.Description;
                outletProductTranslation.LastModifiedDate = DateTime.UtcNow;
            }
            else
            {
                var newOutletProductTranslation = new OutletItemTranslations
                {
                    Title = model.Title,
                    Description = model.Description,
                    Language = model.Language,
                    OutletItemId = outletProduct.Id
                };

                await this.context.OutletTranslations.AddAsync(newOutletProductTranslation.FillCommonProperties());
            }

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetOutletServiceModel { Id = outletProduct.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }

        public async Task<OutletServiceModel> CreateAsync(CreateOutletServiceModel model)
        {
            var outletItem = new OutletItem
            {
                WarehouseId = model.WarehouseId.Value,
                ProductId = model.ProductId.Value,
                ProductName = model.ProductName,
                ProductSku = model.ProductSku,
                Quantity = model.Quantity,
                AvailableQuantity = model.AvailableQuantity,
                Ean = model.Ean,
                SellerId = model.OrganisationId.Value
            };

            this.context.Outlet.Add(outletItem.FillCommonProperties());

            var outletItemTranslation = new OutletItemTranslations
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

        public async Task SyncOutletProducts(UpdateOutletProductsServiceModel model)
        {
            var outletProducts = this.context.Outlet
                .Join(this.context.Warehouses, outlet => outlet.WarehouseId, warehouse => warehouse.Id, (outlet, warehouse) => 
                new
                {
                    Id = outlet.Id,
                    ProductSku = outlet.ProductSku,
                    WarehouseName = warehouse.Name,
                    Ean = outlet.Ean
                });


            foreach (var item in model.OutletItems.OrEmptyIfNull())
            {
                var outlet = await outletProducts.FirstOrDefaultAsync(x => x.ProductSku == item.ProductSku && x.WarehouseName == item.WarehouseName);

                if (outlet is not null)
                {
                    var outletItem = await this.context.Outlet.FirstOrDefaultAsync(x => x.Id == outlet.Id);

                    if (outletItem is not null)
                    {
                        outletItem.AvailableQuantity = item.AvailableQuantity;
                        outletItem.Quantity = item.Quantity;
                        outletItem.Ean = item.Ean;
                        outletItem.LastModifiedDate = DateTime.UtcNow;
                    }
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
                            Ean = item.Ean,
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
            var outletItem = await this.context.Outlet.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);
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
                    Ean = outletItem.Ean,
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
            var outletItems = this.context.Outlet.Where(x => x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                var warehouse = await this.context.Warehouses.FirstOrDefaultAsync(x => x.Name.StartsWith(model.SearchTerm));

                outletItems = outletItems.Where(x => x.Translations.Any(x => x.Title.StartsWith(model.SearchTerm) || x.Description.StartsWith(model.SearchTerm)) || x.WarehouseId == warehouse.Id);
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
                    Ean = outletItem.Ean,
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
            var outletItems = this.context.Outlet.Where(x => model.Ids.Contains(x.Id) && x.IsActive);

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
                    Ean = outletItem.Ean,
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
                              join warehouse in this.context.Warehouses on o.WarehouseId equals warehouse.Id
                              join ot in this.context.OutletTranslations on o.Id equals ot.OutletItemId
                              where o.ProductId == model.ProductId.Value && o.IsActive
                              select new
                              {
                                  Id = o.Id,
                                  ProductId = o.ProductId,
                                  ProductName = o.ProductName,
                                  ProductSku = o.ProductSku,
                                  Quantity = o.Quantity,
                                  AvailableQuantity = o.AvailableQuantity,
                                  WarehouseId = o.WarehouseId,
                                  WarehouseName = warehouse.Name,
                                  Ean = o.Ean,
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
                    Ean = outletItems.FirstOrDefault().Ean,
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
                              join warehouse in this.context.Warehouses on o.WarehouseId equals warehouse.Id
                              join ot in this.context.OutletTranslations on o.Id equals ot.OutletItemId
                              where o.ProductSku == model.ProductSku && o.IsActive
                              select new
                              {
                                  Id = o.Id,
                                  ProductId = o.ProductId,
                                  ProductName = o.ProductName,
                                  ProductSku = o.ProductSku,
                                  Quantity = o.Quantity,
                                  AvailableQuantity = o.AvailableQuantity,
                                  WarehouseId = o.WarehouseId,
                                  WarehouseName = warehouse.Name,
                                  Ean = o.Ean,
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
                    Ean = outletItems.FirstOrDefault().Ean,
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
            var outletItems = (from i in this.context.Outlet
                               group i by new { i.ProductId } into gpi
                               where gpi.Sum(x => x.AvailableQuantity) > 0
                               select new OutletSumServiceModel
                               {
                                   ProductId = gpi.Key.ProductId,
                                   ProductName = gpi.FirstOrDefault().ProductName,
                                   ProductSku = gpi.FirstOrDefault().ProductSku,
                                   Ean = gpi.FirstOrDefault().Ean,
                                   AvailableQuantity = gpi.Sum(x => x.AvailableQuantity),
                                   Quantity = gpi.Sum(x => x.Quantity)
                               }).OrderByDescending(x => x.AvailableQuantity);

            var pagedResults = outletItems.PagedIndex(new Pagination(outletItems.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedItemsServiceModel = new PagedResults<IEnumerable<OutletSumServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var outlet = new List<OutletSumServiceModel>();

            foreach (var outletItem in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var item = new OutletSumServiceModel
                {
                    ProductId = outletItem.ProductId,
                    ProductName = outletItem.ProductName,
                    ProductSku = outletItem.ProductSku,
                    AvailableQuantity = outletItem.AvailableQuantity,
                    Quantity = outletItem.Quantity,
                    Ean = outletItem.Ean
                };

                var outletProduct = await this.context.Outlet.FirstOrDefaultAsync(x => x.ProductId == outletItem.ProductId);
                
                if (outletProduct is not null)
                {
                    var itemTranslation = await this.context.OutletTranslations.FirstOrDefaultAsync(x => x.OutletItemId == outletProduct.Id && x.Language == model.Language && x.IsActive);

                    if (itemTranslation is null)
                    {
                        itemTranslation = await this.context.OutletTranslations.FirstOrDefaultAsync(x => x.OutletItemId == outletProduct.Id && x.IsActive);
                    }

                    item.Title = itemTranslation?.Title;
                    item.Description = itemTranslation?.Description;

                    outlet.Add(item);
                }
            }

            pagedItemsServiceModel.Data = outlet;

            return pagedItemsServiceModel;
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

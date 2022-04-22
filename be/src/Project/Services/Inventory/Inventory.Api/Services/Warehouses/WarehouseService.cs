using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Inventory.Api.Infrastructure;
using Inventory.Api.ServicesModels.WarehouseServiceModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Inventory.Api.Services.Warehouses
{
    public class WarehouseService : IWarehouseService
    {
        private readonly InventoryContext context;
        private readonly IStringLocalizer warehouseLocalizer;

        public WarehouseService(
            InventoryContext context,
            IStringLocalizer<InventoryResources> warehouseLocalizer)
        {
            this.context = context;
            this.warehouseLocalizer = warehouseLocalizer;
        }

        public async Task<WarehouseServiceModel> CreateAsync(CreateWarehouseServiceModel serviceModel)
        {
            var warehouse = new Infrastructure.Entities.Warehouse
            {
                Name = serviceModel.Name,
                Location = serviceModel.Location,
                SellerId = serviceModel.OrganisationId.Value
            };

            this.context.Warehouses.Add(warehouse.FillCommonProperties());

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetWarehouseServiceModel { Id = warehouse.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }

        public async Task<PagedResults<IEnumerable<WarehouseServiceModel>>> GetAsync(GetWarehousesServiceModel model)
        {
            var warehouses = from c in this.context.Warehouses
                             where c.SellerId == model.OrganisationId.Value && c.IsActive
                             select new WarehouseServiceModel
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 Location = c.Location,
                                 LastModifiedDate = c.LastModifiedDate,
                                 CreatedDate = c.CreatedDate
                             };


            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                warehouses = warehouses.Where(x => x.Name.StartsWith(model.SearchTerm) || x.Location.StartsWith(model.SearchTerm));
            }

            warehouses = warehouses.ApplySort(model.OrderBy);

            return warehouses.PagedIndex(new Pagination(warehouses.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<WarehouseServiceModel> GetAsync(GetWarehouseServiceModel model)
        {
            var warehouse = from c in this.context.Warehouses
                            where c.SellerId == model.OrganisationId.Value && c.Id == model.Id && c.IsActive
                            select new WarehouseServiceModel
                          {
                              Id = c.Id,
                              Name = c.Name,
                              Location = c.Location,
                              LastModifiedDate = c.LastModifiedDate,
                              CreatedDate = c.CreatedDate
                          };

            return await warehouse.FirstOrDefaultAsync();
        }

        public async Task<WarehouseServiceModel> GetAsync(GetWarehouseByNameServiceModel model)
        {
            var warehouse = from c in this.context.Warehouses
                            where c.Name == model.Name && c.IsActive
                            select new WarehouseServiceModel
                            {
                                Id = c.Id,
                                Name = c.Name,
                                Location = c.Location,
                                LastModifiedDate = c.LastModifiedDate,
                                CreatedDate = c.CreatedDate
                            };

            return await warehouse.FirstOrDefaultAsync();
        }

        public async Task<PagedResults<IEnumerable<WarehouseServiceModel>>> GetByIdsAsync(GetWarehousesByIdsServiceModel model)
        {
            var warehouses = from c in this.context.Warehouses
                          where model.Ids.Contains(c.Id) && c.SellerId == model.OrganisationId.Value && c.IsActive
                          select new WarehouseServiceModel
                          {
                              Id = c.Id,
                              Name = c.Name,
                              Location = c.Location,
                              LastModifiedDate = c.LastModifiedDate,
                              CreatedDate = c.CreatedDate
                          };

            return warehouses.PagedIndex(new Pagination(warehouses.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task DeleteAsync(DeleteWarehouseServiceModel model)
        {
            var warehouse = await this.context.Warehouses.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);
            if (warehouse == null)
            {
                throw new CustomException(this.warehouseLocalizer.GetString("WarehouseNotFound"), (int)HttpStatusCode.NotFound);
            }

            if (await this.context.Inventory.AnyAsync(x => x.WarehouseId == warehouse.Id && x.IsActive))
            {
                throw new CustomException(this.warehouseLocalizer.GetString("WarehouseDeleteInventoryConflict"), (int)HttpStatusCode.Conflict);
            }

            warehouse.IsActive = false;
            await this.context.SaveChangesAsync();
        }

        public async Task<WarehouseServiceModel> UpdateAsync(UpdateWarehouseServiceModel serviceModel)
        {
            var warehouse = await this.context.Warehouses.FirstOrDefaultAsync(x => x.Id == serviceModel.Id && x.SellerId == serviceModel.OrganisationId.Value && x.IsActive);
            if (warehouse == null)
            {
                throw new CustomException(this.warehouseLocalizer.GetString("WarehouseNotFound"), (int)HttpStatusCode.NotFound);
            }

            warehouse.Name = serviceModel.Name;
            warehouse.Location = serviceModel.Location;
            warehouse.LastModifiedDate = DateTime.UtcNow;

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetWarehouseServiceModel { Id = warehouse.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }
    }
}

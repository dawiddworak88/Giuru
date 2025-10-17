using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Managers.Entities;
using Client.Api.ServicesModels.Managers;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;

namespace Client.Api.Services.Managers
{
    public class ClientAccountManagersService : IClientAccountManagersService
    {
        private readonly ClientContext context;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;

        public ClientAccountManagersService(
            ClientContext context,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            this.context = context;
            this.clientLocalizer = clientLocalizer;
        }

        public async Task<Guid> CreateAsync(CreateClientAccountManagerServiceModel model)
        {
            var existingManager = await this.context.ClientAccountManagers.FirstOrDefaultAsync(x => x.Email == model.Email && x.IsActive);

            if (existingManager is not null)
            {
                throw new ConflictException(this.clientLocalizer.GetString("ManagerExist"));
            }

            var manager = new ClientAccountManager
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            await this.context.ClientAccountManagers.AddAsync(manager.FillCommonProperties());
            await this.context.SaveChangesAsync();

            return manager.Id;
        }

        public async Task DeleteAsync(DeleteClientAccountManagerServiceModel model)
        {
            var manager = await this.context.ClientAccountManagers.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (manager is null)
            {
                throw new NotFoundException(this.clientLocalizer.GetString("ManagerNotFound"));
            }

            if (await this.context.ClientsAccountManagers.AnyAsync(x => x.ClientManagerId == model.Id && x.IsActive))
            {
                throw new ConflictException(this.clientLocalizer.GetString("ManagerDeleteConflict"));
            }

            manager.IsActive = false;

            await this.context.SaveChangesAsync();
        }

        public async Task<ClientAccountManagerServiceModel> GetAsync(GetClientAccountManagerServiceModel model)
        {
            var manager = await this.context.ClientAccountManagers.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (manager is null)
            {
                throw new NotFoundException(this.clientLocalizer.GetString("ManagerNotFound"));
            }

            return new ClientAccountManagerServiceModel
            {
                Id = model.Id,
                FirstName = manager.FirstName,
                LastName = manager.LastName,
                Email = manager.Email,
                PhoneNumber = manager.PhoneNumber,
                LastModifiedDate = manager.LastModifiedDate,
                CreatedDate = manager.CreatedDate
            };
        }

        public async Task<PagedResults<IEnumerable<ClientAccountManagerServiceModel>>> GetAsync(GetClientAccountManagersServiceModel model)
        {
            var managers = from m in this.context.ClientAccountManagers
                           where m.IsActive
                           select new ClientAccountManagerServiceModel
                           {
                               Id = m.Id,
                               FirstName = m.FirstName,
                               LastName = m.LastName,
                               Email = m.Email,
                               PhoneNumber = m.PhoneNumber,
                               LastModifiedDate = m.LastModifiedDate,
                               CreatedDate = m.CreatedDate
                           };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                managers = managers.Where(x => x.FirstName.StartsWith(model.SearchTerm) || x.LastName.StartsWith(model.SearchTerm) || x.Email.StartsWith(model.SearchTerm));
            }

            managers = managers.ApplySort(model.OrderBy);

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                managers = managers.Take(Constants.MaxItemsPerPageLimit);

                return managers.PagedIndex(new Pagination(managers.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }

            return managers.PagedIndex(new Pagination(managers.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
        }

        public async Task<PagedResults<IEnumerable<ClientAccountManagerServiceModel>>> GetByIdsAsync(GetClientAccountManagersByIdsServiceModel model)
        {
            var managers = from m in this.context.ClientAccountManagers
                           where model.Ids.Contains(m.Id)
                           select new ClientAccountManagerServiceModel
                           {
                               Id = m.Id,
                               FirstName = m.FirstName,
                               LastName = m.LastName,
                               Email = m.Email,
                               PhoneNumber = m.PhoneNumber,
                               LastModifiedDate = m.LastModifiedDate,
                               CreatedDate = m.CreatedDate
                           };

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                managers = managers.Take(Constants.MaxItemsPerPageLimit);

                return managers.PagedIndex(new Pagination(managers.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            } 

            return managers.PagedIndex(new Pagination(managers.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
        }

        public async Task<Guid> UpdateAsync(UpdateClientAccountManagerServiceModel model)
        {
            var manager = await this.context.ClientAccountManagers.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (manager is null)
            {
                throw new NotFoundException(this.clientLocalizer.GetString("ManagerNotFound"));
            }

            manager.FirstName = model.FirstName;
            manager.LastName = model.LastName;
            manager.Email = model.Email;
            manager.PhoneNumber = model.PhoneNumber;
            manager.LastModifiedDate = DateTime.UtcNow;

            await this.context.SaveChangesAsync();

            return manager.Id;
        }
    }
}

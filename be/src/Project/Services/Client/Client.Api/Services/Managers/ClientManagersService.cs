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

namespace Client.Api.Services.Managers
{
    public class ClientManagersService : IClientManagersService
    {
        private readonly ClientContext context;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;

        public ClientManagersService(
            ClientContext context,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            this.context = context;
            this.clientLocalizer = clientLocalizer;
        }

        public async Task<Guid> CreateAsync(CreateClientManagerServiceModel model)
        {
            var existingManager = await this.context.ClientManagers.FirstOrDefaultAsync(x => x.Email == model.Email && x.IsActive);

            if (existingManager is not null)
            {
                throw new CustomException(this.clientLocalizer.GetString("ManagerIsExisting"), (int)HttpStatusCode.BadRequest);
            }

            var manager = new ClientManager
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            await this.context.ClientManagers.AddAsync(manager.FillCommonProperties());
            await this.context.SaveChangesAsync();

            return manager.Id;
        }

        public async Task DeleteAsync(DeleteClientManagerServiceModel model)
        {
            var manager = await this.context.ClientManagers.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (manager is null)
            {
                throw new CustomException(this.clientLocalizer.GetString("ManagerNotFound"), (int)HttpStatusCode.NotFound);
            }

            if (await this.context.ClientsManagers.AnyAsync(x => x.ClientManagerId == model.Id && x.IsActive))
            {
                throw new CustomException(this.clientLocalizer.GetString("ManagerDeleteConflict"), (int)HttpStatusCode.Conflict);
            }

            manager.IsActive = false;

            await this.context.SaveChangesAsync();
        }

        public async Task<ClientManagerServiceModel> GetAsync(GetClientManagerServiceModel model)
        {
            var manager = await this.context.ClientManagers.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (manager is null)
            {
                throw new CustomException(this.clientLocalizer.GetString("ManagerNotFound"), (int)HttpStatusCode.NotFound);
            }

            return new ClientManagerServiceModel
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

        public async Task<PagedResults<IEnumerable<ClientManagerServiceModel>>> GetAsync(GetClientManagersServiceModel model)
        {
            var managers = from m in this.context.ClientManagers
                           where m.IsActive
                           select new ClientManagerServiceModel
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

            return managers.PagedIndex(new Pagination(managers.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public Task<PagedResults<IEnumerable<ClientManagerServiceModel>>> GetByIdsAsync(GetClientManagersByIdsServiceModel model)
        {
            var managers = from m in this.context.ClientManagers
                           where model.Ids.Contains(m.Id)
                           select new ClientManagerServiceModel
                           {
                               Id = m.Id,
                               FirstName = m.FirstName,
                               LastName = m.LastName,
                               Email = m.Email,
                               PhoneNumber = m.PhoneNumber,
                               LastModifiedDate = m.LastModifiedDate,
                               CreatedDate = m.CreatedDate
                           };

            return managers.PagedIndex(new Pagination(managers.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<Guid> UpdateAsync(UpdateClientManagerServiceModel model)
        {
            var manager = await this.context.ClientManagers.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (manager is null)
            {
                throw new CustomException(this.clientLocalizer.GetString("ManagerNotFound"), (int)HttpStatusCode.NotFound);
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

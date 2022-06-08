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
using System.Threading.Tasks;

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

            await this.context.AddAsync(manager.FillCommonProperties());
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
            throw new NotImplementedException();
        }

        public Task<PagedResults<IEnumerable<ClientManagerServiceModel>>> GetByIdsAsync(GetClientManagersByIdsServiceModel model)
        {
            throw new NotImplementedException();
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

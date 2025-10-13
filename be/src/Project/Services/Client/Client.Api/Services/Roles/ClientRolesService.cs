using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Roles.Entities;
using Client.Api.ServicesModels.Roles;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;

namespace Client.Api.Services.Roles
{
    public class ClientRolesService : IClientRolesService
    {
        private readonly ClientContext context;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;

        public ClientRolesService(
            ClientContext context,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            this.context = context;
            this.clientLocalizer = clientLocalizer;
        }

        public async Task<Guid> CreateAsync(CreateClientRoleServiceModel model)
        {
            var existingRole = await this.context.ClientRoles.FirstOrDefaultAsync(x => x.Name == model.Name && x.IsActive);

            if (existingRole is not null)
            {
                throw new ConflictException(this.clientLocalizer.GetString("RoleIsExisting"));
            }

            var role = new ClientRole
            {
                Name = model.Name
            };

            await this.context.ClientRoles.AddAsync(role.FillCommonProperties());
            await this.context.SaveChangesAsync();

            return role.Id;
        }

        public async Task DeleteAsync(DeleteClientRoleServiceModel model)
        {
            var role = await this.context.ClientRoles.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (role is null)
            {
                throw new NotFoundException(this.clientLocalizer.GetString("RoleNotFound"));
            }

            role.IsActive = false;
            
            await this.context.SaveChangesAsync();
        }

        public async Task<PagedResults<IEnumerable<ClientRoleServiceModel>>> GetAsync(GetClientRolesServiceModel model)
        {
            var roles = from r in this.context.ClientRoles
                        where r.IsActive
                        select new ClientRoleServiceModel
                        {
                            Id = r.Id,
                            Name = r.Name,
                            LastModifiedDate = r.LastModifiedDate,
                            CreatedDate = r.CreatedDate
                        };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                roles = roles.Where(x => x.Name.StartsWith(model.SearchTerm));
            }

            roles = roles.ApplySort(model.OrderBy);

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                roles = roles.Take(Constants.MaxItemsPerPageLimit);

                return roles.PagedIndex(new Pagination(roles.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }

            return roles.PagedIndex(new Pagination(roles.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
        }

        public async Task<ClientRoleServiceModel> GetAsync(GetClientRoleServiceModel model)
        {
            var role = await this.context.ClientRoles.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (role is null)
            {
                throw new NotFoundException(this.clientLocalizer.GetString("RoleNotFound"));
            }

            return new ClientRoleServiceModel
            {
                Id = role.Id,
                Name = role.Name,
                LastModifiedDate = role.LastModifiedDate,
                CreatedDate = role.CreatedDate
            };
        }

        public async Task<PagedResults<IEnumerable<ClientRoleServiceModel>>> GetByIdsAsync(GetClientRolesByIdsServiceModel model)
        {
            var roles = from r in this.context.ClientRoles
                        where model.Ids.Contains(r.Id) && r.IsActive
                        select new ClientRoleServiceModel
                        {
                            Id = r.Id,
                            Name = r.Name,
                            LastModifiedDate = r.LastModifiedDate,
                            CreatedDate = r.CreatedDate
                        };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                roles = roles.Where(x => x.Name.StartsWith(model.SearchTerm));
            }

            roles = roles.ApplySort(model.OrderBy);

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                roles = roles.Take(Constants.MaxItemsPerPageLimit);

                return roles.PagedIndex(new Pagination(roles.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }

            return roles.PagedIndex(new Pagination(roles.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
        }

        public async Task<Guid> UpdateAsync(UpdateClientRoleServiceModel model)
        {
            var role = await this.context.ClientRoles.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (role is null)
            {
                throw new NotFoundException(this.clientLocalizer.GetString("RoleNotFound"));
            }

            role.Name = model.Name;
            role.LastModifiedDate = DateTime.UtcNow;

            await this.context.SaveChangesAsync();

            return role.Id;
        }
    }
}

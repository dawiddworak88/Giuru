using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Foundation.GenericRepository.Paginations;
using Foundation.Extensions.Exceptions;
using System.Net;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Foundation.Localization.Services;
using Identity.Api.Infrastructure;
using Identity.Api.v1.Areas.Clients.Services;
using Identity.Api.v1.Areas.Clients.ResultModels;
using Identity.Api.v1.Areas.Clients.Models;
using Identity.Api.Infrastructure.Clients.Entities;
using Foundation.GenericRepository.Extensions;

namespace Identity.Api.v1.Areas.Clients.Services
{
    public class ClientsService : IClientsService
    {
        private readonly IdentityContext context;
        private readonly ICultureService cultureService;
        private readonly IStringLocalizer clientLocalizer;

        public ClientsService(
            IdentityContext context,
            ICultureService cultureService,
            IStringLocalizer<ClientResources> clientLocalizer)
        {
            this.context = context;
            this.cultureService = cultureService;
            this.clientLocalizer = clientLocalizer;
        }

        public async Task<PagedResults<IEnumerable<ClientResultModel>>> GetAsync(GetClientsModel model)
        {
            var categories = from c in this.context.Clients
                             where c.SellerId == model.OrganisationId.Value && c.IsActive
                             select new ClientResultModel
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 Email = c.Email,
                                 CommunicationLanguage = c.Language,
                                 LastModifiedDate = c.LastModifiedDate,
                                 CreatedDate = c.CreatedDate
                             };

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                categories = categories.Where(x => x.Name.StartsWith(model.SearchTerm));
            }

            return categories.PagedIndex(new Pagination(categories.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<ClientResultModel> GetAsync(GetClientModel model)
        {
            var categories = from c in this.context.Clients
                             where c.SellerId == model.OrganisationId.Value && c.IsActive
                             select new ClientResultModel
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 Email = c.Email,
                                 CommunicationLanguage = c.Language,
                                 LastModifiedDate = c.LastModifiedDate,
                                 CreatedDate = c.CreatedDate
                             };

            return await categories.FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(DeleteClientModel model)
        {
            this.cultureService.SetCulture(model.Language);

            var client = await this.context.Clients.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (client == null)
            {
                throw new CustomException(this.clientLocalizer.GetString("ClientNotFound"), (int)HttpStatusCode.NotFound);
            }

            client.IsActive = false;

            await this.context.SaveChangesAsync();
        }

        public async Task<ClientResultModel> UpdateAsync(UpdateClientModel serviceModel)
        {
            this.cultureService.SetCulture(serviceModel.Language);

            var client = await this.context.Clients.FirstOrDefaultAsync(x => x.Id == serviceModel.Id && x.SellerId == serviceModel.OrganisationId.Value && x.IsActive);

            if (client == null)
            {
                throw new CustomException(this.clientLocalizer.GetString("ClientNotFound"), (int)HttpStatusCode.NotFound);
            }

            client.Name = serviceModel.Name;
            client.Email = serviceModel.Email;
            client.Language = serviceModel.CommunicationLanguage;

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetClientModel { Id = client.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }

        public async Task<ClientResultModel> CreateAsync(CreateClientModel serviceModel)
        {
            this.cultureService.SetCulture(serviceModel.Language);

            var client = new Client
            {
                Name = serviceModel.Name,
                Email = serviceModel.Email,
                Language = serviceModel.CommunicationLanguage,
                SellerId = serviceModel.OrganisationId.Value
            };

            this.context.Clients.Add(client.FillCommonProperties());

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetClientModel { Id = client.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }
    }
}
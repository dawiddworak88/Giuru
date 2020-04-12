using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Client.Api.v1.Validators;
using Feature.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Database.Areas.Tenants.Entities;
using Foundation.Database.Shared.Repositories;
using Foundation.TenantDatabase.Shared.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class ClientController : BaseApiController
    {
        private readonly IGenericRepository<Tenant> tenantRepository;
        private readonly TenantGenericRepositoryFactory genericRepositoryFactory;

        public ClientController(
            IGenericRepository<Tenant> tenantRepository, 
            TenantGenericRepositoryFactory genericRepositoryFactory)
        {
            this.tenantRepository = tenantRepository;
            this.genericRepositoryFactory = genericRepositoryFactory;
        }

        /// <summary>
        /// Creates a new client
        /// </summary>
        /// <param name="clientModel">Client to save.</param>
        /// <returns>Account creation results.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Create([FromBody] ClientRequestModel clientModel)
        {
            var validator = new ClientRequestModelValidator();
            var validationResult = await validator.ValidateAsync(clientModel);

            if (validationResult.IsValid)
            {
                var tenantIdClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.TenantIdClaim);

                if (tenantIdClaim != null)
                {
                    var tenant = this.tenantRepository.GetById(Guid.Parse(tenantIdClaim.Value));

                    if (tenant != null)
                    {
                        if (!clientModel.Id.HasValue)
                        {
                            var client = new Foundation.TenantDatabase.Areas.Clients.Entities.Client
                            {
                                Language = clientModel.Language,
                                Name = clientModel.Name,
                                IsActive = true,
                                LastModifiedBy = this.User.Identity.Name,
                                LastModifiedDate = DateTime.UtcNow,
                                CreatedBy = this.User.Identity.Name,
                                CreatedDate = DateTime.UtcNow
                            };

                            var clientRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Clients.Entities.Client>(tenant.DatabaseConnectionString);

                            await clientRepository.CreateAsync(client);

                            clientRepository.SaveChanges();

                            var savedClient = clientRepository.GetById(client.Id);

                            return this.CreatedAtAction("GetById", new { id = savedClient.Id }, new ClientResponseModel(savedClient));
                        }
                    }
                }

                return this.BadRequest();
            }

            return this.BadRequest(validationResult.Errors.ToString());
        }
    }
}

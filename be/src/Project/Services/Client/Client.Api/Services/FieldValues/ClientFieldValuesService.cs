using Client.Api.Infrastructure;
using Client.Api.Infrastructure.Fields;
using Client.Api.ServicesModels.FieldValues;
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
using System.Net;
using System.Threading.Tasks;

namespace Client.Api.Services.FieldValues
{
    public class ClientFieldValuesService : IClientFieldValuesService
    {
        private readonly ClientContext _context;
        private readonly IStringLocalizer<ClientResources> _clientLocalizer;

        public ClientFieldValuesService(
            IStringLocalizer<ClientResources> clientLocalizer,
            ClientContext context)
        {
            _context = context;
            _clientLocalizer = clientLocalizer;
        }

        public async Task CreateAsync(CreateClientFieldValuesServiceModel model)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == model.ClientId && x.IsActive);

            if (client is null)
            {
                throw new CustomException(_clientLocalizer.GetString("ClientNotFound"), (int)HttpStatusCode.NoContent);
            }

            var fieldsValues = _context.ClientFieldValues
                    .Include(x => x.Translation)
                    .Where(x => x.ClientId == model.ClientId && model.FieldsValues.Select(y => y.FieldDefinitionId).Contains(x.FieldDefinitionId))
                    .AsSingleQuery()
                    .ToList();

            foreach (var fieldValue in model.FieldsValues.OrEmptyIfNull())
            {
                if (string.IsNullOrWhiteSpace(fieldValue.FieldValue) is false)
                {
                    var existingFieldValue = fieldsValues.FirstOrDefault(x => x.FieldDefinitionId == fieldValue.FieldDefinitionId);

                    if (existingFieldValue is null)
                    {
                        var newFieldValue = new ClientFieldValue
                        {
                            ClientId = model.ClientId.Value,
                            FieldDefinitionId = fieldValue.FieldDefinitionId.Value
                        };

                        await _context.ClientFieldValues.AddAsync(newFieldValue.FillCommonProperties());

                        var newFieldValueTranslation = new ClientFieldValueTranslation
                        {
                            ClientFieldValueId = newFieldValue.Id,
                            FieldValue = fieldValue.FieldValue,
                            Language = model.Language
                        };

                        await _context.ClientFieldValueTranslations.AddAsync(newFieldValueTranslation.FillCommonProperties());
                    }
                    else
                    {
                        var existingTranslation = existingFieldValue.Translation.FirstOrDefault(x => x.Language == model.Language);

                        if (existingTranslation is null)
                        {
                            var newFieldValueTranslation = new ClientFieldValueTranslation
                            {
                                ClientFieldValueId = existingFieldValue.Id,
                                FieldValue = fieldValue.FieldValue,
                                Language = model.Language
                            };

                            await _context.ClientFieldValueTranslations.AddAsync(newFieldValueTranslation.FillCommonProperties());
                        }
                        else
                        {
                            existingTranslation.FieldValue = fieldValue.FieldValue;
                            existingTranslation.LastModifiedDate = DateTime.UtcNow;
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public PagedResults<IEnumerable<ClientFieldValueServiceModel>> Get(GetClientFieldValuesServiceModel model)
        {
            var fieldValues = _context.ClientFieldValues.
                Include(x => x.Translation)
                .Where(x => x.IsActive).AsSingleQuery();

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                fieldValues = fieldValues.Where(x => x.Translation.Any(y => y.FieldValue.StartsWith(model.SearchTerm)));
            }

            if (model.ClientId.HasValue)
            {
                fieldValues = fieldValues.Where(x => x.ClientId == model.ClientId);
            }

            fieldValues = fieldValues.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<ClientFieldValue>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                fieldValues = fieldValues.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = fieldValues.PagedIndex(new Pagination(fieldValues.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = fieldValues.PagedIndex(new Pagination(fieldValues.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return new PagedResults<IEnumerable<ClientFieldValueServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new ClientFieldValueServiceModel
                {
                    Id = x.Id,
                    FieldValue = x.Translation?.FirstOrDefault(x => x.Language == model.Language && x.IsActive)?.FieldValue ?? x.Translation?.FirstOrDefault(x => x.IsActive)?.FieldValue,
                    FieldDefinitionId = x.FieldDefinitionId,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }
    }
}

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
                throw new NotFoundException(_clientLocalizer.GetString("ClientNotFound"));
            }

            var fieldsValues = _context.ClientFieldValues
                    .Include(x => x.Translation)
                    .Where(x => x.ClientId == model.ClientId && model.FieldsValues.Select(y => y.FieldDefinitionId).Contains(x.FieldDefinitionId))
                    .AsSingleQuery()
                    .ToList();

            foreach (var fieldValue in model.FieldsValues.OrEmptyIfNull())
            {
                var existingFieldValue = fieldsValues.FirstOrDefault(x => x.FieldDefinitionId == fieldValue.FieldDefinitionId);

                if (string.IsNullOrWhiteSpace(fieldValue.FieldValue) is true && existingFieldValue is not null)
                {
                    var existingTranslation = existingFieldValue.Translation.FirstOrDefault(x => x.Language == model.Language);

                    if (existingTranslation is not null)
                    {
                        _context.Remove(existingTranslation);
                    }

                    continue;
                }

                if (existingFieldValue is null)
                {
                    var newFieldValue = new ClientFieldValue
                    {
                        ClientId = model.ClientId.Value,
                        FieldDefinitionId = fieldValue.FieldDefinitionId.Value
                    };

                    _context.ClientFieldValues.Add(newFieldValue.FillCommonProperties());

                    if (string.IsNullOrWhiteSpace(fieldValue.FieldValue) is false)
                    {
                        var newFieldValueTranslation = new ClientFieldValueTranslation
                        {
                            ClientFieldValueId = newFieldValue.Id,
                            FieldValue = fieldValue.FieldValue,
                            Language = model.Language
                        };

                        _context.ClientFieldValueTranslations.Add(newFieldValueTranslation.FillCommonProperties());
                    }
                }
                else
                {
                    var existingTranslation = existingFieldValue.Translation?.FirstOrDefault(x => x.Language == model.Language);

                    if (existingTranslation is null)
                    {
                        if (string.IsNullOrWhiteSpace(fieldValue.FieldValue) is false)
                        {
                            var newFieldValueTranslation = new ClientFieldValueTranslation
                            {
                                ClientFieldValueId = existingFieldValue.Id,
                                FieldValue = fieldValue.FieldValue,
                                Language = model.Language
                            };

                            _context.ClientFieldValueTranslations.Add(newFieldValueTranslation.FillCommonProperties());
                        }
                    }
                    else
                    {
                        existingTranslation.FieldValue = fieldValue.FieldValue;
                        existingTranslation.LastModifiedDate = DateTime.UtcNow;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public PagedResults<IEnumerable<ClientFieldValueServiceModel>> Get(GetClientFieldValuesServiceModel model)
        {
            var fieldValues = _context.ClientFieldValues
                .Include(x => x.FieldDefinition)
                .ThenInclude(x => x.FieldDefinitionTranslations)
                .Include(x => x.Translation)
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
                    FieldName = x.FieldDefinition?.FieldDefinitionTranslations?.FirstOrDefault(y => y.Language == model.Language && y.IsActive)?.FieldName ?? x.FieldDefinition?.FieldDefinitionTranslations?.FirstOrDefault(y => y.IsActive)?.FieldName,
                    FieldValue = x.Translation?.FirstOrDefault(x => x.Language == model.Language && x.IsActive)?.FieldValue ?? x.Translation?.FirstOrDefault(x => x.IsActive)?.FieldValue,
                    FieldDefinitionId = x.FieldDefinitionId,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.KeyValues.Caching;
using CleanArchitecture.Blazor.Application.Features.KeyValues.Commands.AddEdit;
using CleanArchitecture.Blazor.Application.Features.KeyValues.DTOs;
using Mgr.Core.Models;

namespace CleanArchitecture.Blazor.Application.Features.KeyValues.Commands.Import;

public class ImportKeyValuesCommand : ICacheInvalidatorRequest<MethodResult>
{
    public string FileName { get; set; }
    public byte[] Data { get; set; }
    public CancellationTokenSource? SharedExpiryTokenSource => KeyValueCacheKey.SharedExpiryTokenSource();
    public ImportKeyValuesCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record CreateKeyValueTemplateCommand : IRequest<byte[]>
{

}
public class ImportKeyValuesCommandHandler :
    IRequestHandler<CreateKeyValueTemplateCommand, byte[]>,
    IRequestHandler<ImportKeyValuesCommand, MethodResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ImportKeyValuesCommandHandler> _localizer;
    private readonly IValidator<AddEditKeyValueCommand> _addValidator;

    public ImportKeyValuesCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        IExcelService excelService,
        IStringLocalizer<ImportKeyValuesCommandHandler> localizer,
        IValidator<AddEditKeyValueCommand> addValidator
        )
    {
        _context = context;
        _mapper = mapper;
        _excelService = excelService;
        _localizer = localizer;
        _addValidator = addValidator;
    }
    public async Task<MethodResult> Handle(ImportKeyValuesCommand request, CancellationToken cancellationToken)
    {
        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, KeyValue, object?>>
            {
                { _localizer["Name"], (row,item) => item.Name = (Picklist)Enum.Parse(typeof(Picklist),row[_localizer["Name"]].ToString()) },
                { _localizer["Value"], (row,item) => item.Value =  row[_localizer["Value"]]?.ToString() },
                { _localizer["Text"], (row,item) => item.Text =  row[_localizer["Text"]]?.ToString() },
                { _localizer["Description"], (row,item) => item.Description =  row[_localizer["Description"]]?.ToString() }
            }, _localizer["Data"]);

        if (result.Succeeded && result.Data is not null)
        {
            var importItems = result.Data;
            var errors = new List<string>();
            var errorsOccurred = false;
            foreach (var item in importItems)
            {
                var validationMethodResult = await _addValidator.ValidateAsync(new AddEditKeyValueCommand() { Name = item.Name, Value = item.Value, Description = item.Description, Text = item.Text }, cancellationToken);
                if (validationMethodResult.IsValid)
                {
                    var exist = await _context.KeyValues.AnyAsync(x => x.Name == item.Name && x.Value == item.Value, cancellationToken);
                    if (!exist)
                    {
                        item.AddDomainEvent(new CreatedEvent<KeyValue>(item));
                        await _context.KeyValues.AddAsync(item, cancellationToken);
                    }
                }
                else
                {
                    errorsOccurred = true;
                    errors.AddRange(validationMethodResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(item.Name.ToString()) ? $"{item.Name} - " : string.Empty)}{e.ErrorMessage}"));
                }
            }

            if (errorsOccurred)
            {
                return await MethodResult.ErrorBussiness(errors);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return await MethodResult.SuccessAsync();
        }
        else
        {
            return await MethodResult.ErrorBussiness(result.Errors);
        }
    }

    public async Task<byte[]> Handle(CreateKeyValueTemplateCommand request, CancellationToken cancellationToken)
    {
        var fields = 
                _localizer["Name"],
                _localizer["Value"],
                _localizer["Text"],
                _localizer["Description"],
                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer["KeyValues"]);
        return result;
    }
}

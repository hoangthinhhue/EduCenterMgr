// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Blazor.Application.Features.ClassTypes.Caching;
using CleanArchitecture.Blazor.Domain.DTOs.ClassTypes.DTOs;
using CleanArchitecture.Blazor.Domain.Interfaces;
using Mgr.Core.Entities;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace CleanArchitecture.Blazor.Application.Features.ClassTypes.Commands.Import;

public class ImportClassTypesCommand : ICacheInvalidatorRequest<MethodResult>
{
    public string CacheKey => ClassTypeCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => ClassTypeCacheKey.SharedExpiryTokenSource();

    public string FileName { get; }
    public byte[] Data { get; }
    public ImportClassTypesCommand(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
public record CreateClassTypesTemplateCommand : IRequest<byte[]>
{

}

public class ImportClassTypesCommandHandler :
             IRequestHandler<CreateClassTypesTemplateCommand, byte[]>,
             IRequestHandler<ImportClassTypesCommand, MethodResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ImportClassTypesCommandHandler> _localizer;
    private readonly IExcelService _excelService;

    public ImportClassTypesCommandHandler(
        IApplicationDbContext context,
        IExcelService excelService,
        IStringLocalizer<ImportClassTypesCommandHandler> localizer,
        IMapper mapper
        )
    {
        _context = context;
        _localizer = localizer;
        _excelService = excelService;
        _mapper = mapper;
    }
    public async Task<MethodResult> Handle(ImportClassTypesCommand request, CancellationToken cancellationToken)
    {

        var result = await _excelService.ImportAsync(request.Data, mappers: new Dictionary<string, Func<DataRow, ClassTypeDto, object?>>
            {
              { _localizer["Code"], (row,item) => item.Code = row[_localizer["Code"]].ToString() },
              { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]].ToString() },
              { _localizer["Description"], (row,item) => item.Description = row[_localizer["Description"]].ToString() },
              { _localizer["Duration"], (row,item) => item.Duration = (int)row[_localizer["Duration"]] },
            }, _localizer["ClassTypes"]);
        if (result.Success)
        {
            foreach (var dto in result.Data!)
            {
                var item = _mapper.Map<ClassType>(dto);
                await _context.ClassTypes.AddAsync(item, cancellationToken);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return MethodResult.ResultWithSuccess();
        }
        else
        {
            return MethodResult.ResultWithError(result.Message);
        }
    }
    public async Task<byte[]> Handle(CreateClassTypesTemplateCommand request, CancellationToken cancellationToken)
    {
        var fields = new string[] {
                   _localizer["Code"],
                   _localizer["Name"],
                   _localizer["Description"],
                   _localizer["Duration"],
                };
        var result = await _excelService.CreateTemplateAsync(fields, _localizer["ClassTypes"]);
        return result;
    }
}


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System.Reflection.Metadata;
using CleanArchitecture.Blazor.Application.Features.ClassTypes.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.ClassTypes.Queries.Export;

public enum ExportType
{
    Excel,
    PDF
}

public class ExportClassTypesQuery : IRequest<byte[]>
{
    public string OrderBy { get; set; } = "Id";
    public string SortDirection { get; set; } = "Desc";
    public string Keyword { get; set; } = String.Empty;
    public ExportType exportType { get; set; }
}

public class ExportClassTypesQueryHandler :
     IRequestHandler<ExportClassTypesQuery, byte[]>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IExcelService _excelService;
    private readonly IPDFService _pdfService;
    private readonly IStringLocalizer<ExportClassTypesQueryHandler> _localizer;

    public ExportClassTypesQueryHandler(
        ApplicationDbContext context,
        IMapper mapper,
        IExcelService excelService,
        IPDFService pdfService,
        IStringLocalizer<ExportClassTypesQueryHandler> localizer
        )
    {
        _context = context;
        _mapper = mapper;
        _excelService = excelService;
        _pdfService = pdfService;
        _localizer = localizer;
    }
    public async Task<byte[]> Handle(ExportClassTypesQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.ClassTypes.Where(x => x.Name!.Contains(request.Keyword) || x.Description!.Contains(request.Keyword))
                   .OrderBy($"{request.OrderBy} {request.SortDirection}")
                   .ProjectTo<ClassTypeDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

        var mappers = new Dictionary<string, Func<ClassTypeDto, object?>>()
                {
                    { _localizer["Code"], item => item.Code },
                    { _localizer["Name"], item => item.Name },
                    { _localizer["Description"], item => item.Description },
                    { _localizer["Duration"], item => item.Duration },
                };

        byte[]? result;
        switch (request.exportType)
        {
            case ExportType.PDF:
                result = await _pdfService.ExportAsync(data, mappers, _localizer["ClassTypes"], true);
                break;
            default:
                result = await _excelService.ExportAsync(data, mappers, _localizer["ClassTypes"]);
                break;
        }

        return result;
    }
}
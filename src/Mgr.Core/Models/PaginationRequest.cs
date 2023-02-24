// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MessagePack;

namespace Mgr.Core.Models;

/// <summary>  
/// This class contains properites used for paging, sorting, grouping, filtering and will be used as a parameter model  
///   
/// SortOrder   - enum of sorting orders  
/// SortColumn  - Name of the column on which sorting has to be done,  
///               as for now sorting can be performed only on one column at a time.  
/// FilterParams - Filtering can be done on multiple columns and for one column multiple values can be selected  
///               key :- will be column name, Value :- will be array list of multiple values  
/// GroupingColumns - It will contain column names in a sequence on which grouping has been applied   
/// PageNo   - Page No to be displayed in UI, default to 1  
/// PageSize     - Number of items per page, default to 20  
/// </summary>\
[MessagePackObject(keyAsPropertyName: true)]
public class PaginationRequest : InputModel
{
    public PaginationRequest() : base()
    {
    }
    int pageIndex = 1;
    public int PageIndex { get { return pageIndex; } set { if (value > 1) pageIndex = value; } }

    int pageSize = 30;
    public int PageSize { get { return pageSize; } set { if (value > 0) pageSize = value; } }
}

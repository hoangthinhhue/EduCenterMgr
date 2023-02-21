// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;

namespace Mgr.Core.Interface;

public interface IEntity
{

}
public interface IEntity<Tkey>
    where Tkey: struct
{


}
public interface IBaseEntity : IAuditEntity,ISoftDelete

{


}
public interface IAuditEntity : IEntity
{

    DateTime? CreatedDate { get; set; }
    Guid? CreatedBy { get; set; }
    DateTime? UpdatedDate { get; set; }
    Guid? UpdatedBy { get; set; }
}
public interface ISoftDelete : IEntity
{
    bool? IsDeleted { get; set; }
}

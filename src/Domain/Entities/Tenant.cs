using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mgr.Core.Entities;

namespace CleanArchitecture.Blazor.Domain.Entities;
public class Tenant : BaseEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Name { get; set; }
    public string? Description { get; set; }
}

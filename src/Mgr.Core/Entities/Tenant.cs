using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mgr.Core.Entities;
public class Tenant : BaseEntity<Guid>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mgr.Core.Interfaces.Data;
public interface IDbFactory<TDataContext>
    where TDataContext : DbContext
{
    TDataContext DataContext { get; set; }
}

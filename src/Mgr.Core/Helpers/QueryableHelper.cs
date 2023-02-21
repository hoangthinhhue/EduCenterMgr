using Mgr.Core.EnumType;
using Mgr.Core.Extensions;
using Mgr.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mgr.Core.Helpers
{
    public class QueryableHelper<T>
    {
        public static IQueryable<T> GetQuery(IQueryable<T> query, IList<FilterParams> filters)
        {
            var expressions = PredicateBuilder.FromFilter<T>(filters);
            return query.Where(expressions);
        }
    }
}

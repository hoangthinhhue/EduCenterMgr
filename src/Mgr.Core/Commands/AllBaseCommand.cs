using MediatR;
using Mgr.Core.Entities;
using Mgr.Core.Extensions;
using Mgr.Core.Interfaces;
using Mgr.Core.Interfaces.Data;
using Mgr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Uni.Core.Commands;
using Uni.Core.Helper;

namespace Mgr.Core.Commands
{
    public class AllBaseCommand<T, Tkey> : IRequest<IMethodResult<List<T>>>
        where T : BaseEntity<Tkey>
        where Tkey : struct
    {
        public InputModel Params { get; set; }

    }
    public class AllBaseCommandHandler<TDataContext, T, Tkey>
       : BaseCommand<TDataContext, T>, IRequestHandler<AllBaseCommand<T, Tkey>, IMethodResult<List<T>>>
       where TDataContext : DbContext
       where T : BaseEntity<Tkey>
       where Tkey : struct
    {
        private readonly IBaseRepository<TDataContext, T> _repository;

        public AllBaseCommandHandler()
        {
            _repository = HttpContextInfo.GetRequestService<IBaseRepository<TDataContext, T>>();
        }

        public async Task<IMethodResult<List<T>>> Handle(AllBaseCommand<T, Tkey> request, CancellationToken cancellationToken)
        {
            Expression<Func<T, bool>> expression;
            // Filter non-deleted records
            expression = x => x.IsDeleted != true;
            var list = _repository.List(expression);
            return await list.ToAll(request.Params);
        }
    }
}

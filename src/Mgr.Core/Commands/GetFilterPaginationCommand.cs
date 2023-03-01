using MediatR;
using Mgr.Core.Entities;
using Mgr.Core.Extensions;
using Mgr.Core.Interface;
using Mgr.Core.Interfaces;
using Mgr.Core.Interfaces.Data;
using Mgr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Uni.Core.Helper;

namespace Uni.Core.Commands
{
    public class GetFilterPaginationCommand<T, Tkey> : IRequest<IMethodResult<List<T>>>
       where T : IBaseEntity<Tkey>
        where Tkey : struct
    {
        public PaginationRequest PagingParams { get; set; }
    }

    public class GetFilterPaginationCommandHandler<TDataContext, T, Tkey>
       : BaseCommand<TDataContext, T,Tkey>, IRequestHandler<GetFilterPaginationCommand<T, Tkey>, IMethodResult<List<T>>>
       where TDataContext : DbContext
       where T : IBaseEntity<Tkey>
       where Tkey : struct
    {
        private readonly IBaseRepository<TDataContext, T> _repository;

        public GetFilterPaginationCommandHandler()
        {
            _repository = HttpContextInfo.GetRequestService<IBaseRepository<TDataContext, T>>();
        }

        public async Task<IMethodResult<List<T>>> Handle(GetFilterPaginationCommand<T, Tkey> request, CancellationToken cancellationToken)
        {
            var query = _repository.List(x => x.IsDeleted != true);
            return await query.ToMethodReuslt(request.PagingParams);
        }
    }
}


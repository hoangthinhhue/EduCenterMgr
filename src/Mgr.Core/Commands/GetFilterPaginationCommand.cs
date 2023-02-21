using MediatR;
using Mgr.Core.Entities;
using Mgr.Core.Extensions;
using Mgr.Core.Interfaces;
using Mgr.Core.Interfaces.Data;
using Mgr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Uni.Core.Helper;

namespace Uni.Core.Commands
{
    public class GetFilterPaginationCommand<T, Tkey> : IRequest<IMethodResult<IList<T>>>
       where T : BaseEntity<Tkey>
        where Tkey : struct
    {
        public PaginationRequest PagingParams { get; set; }
    }

    public class GetFilterPaginationCommandHandler<TDataContext, T, Tkey>
       : BaseCommand<TDataContext, T>, IRequestHandler<GetFilterPaginationCommand<T, Tkey>, IMethodResult<IList<T>>>
       where TDataContext : DbContext
       where T : BaseEntity<Tkey>
       where Tkey : struct
    {
        private readonly IBaseRepository<TDataContext, T> _repository;

        public GetFilterPaginationCommandHandler()
        {
            _repository = HttpContextInfo.GetRequestService<IBaseRepository<TDataContext, T>>();
        }

        public async Task<IMethodResult<IList<T>>> Handle(GetFilterPaginationCommand<T, Tkey> request, CancellationToken cancellationToken)
        {
            Expression<Func<T, bool>> expression;
            expression = x => x.IsDeleted != true;
            var query = _repository.List(expression);
            return await query.ToMethodReuslt(request.PagingParams);
        }
    }
}


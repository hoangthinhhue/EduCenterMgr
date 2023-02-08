using CleanArchitecture.Blazor.Application.Features.ClassTypes.Queries.Pagination;

namespace CleanArchitecture.Blazor.Application.Features.ClassTypes.Queries.Specification;

public class SearchClassTypeSpecification : Specification<ClassType>
{
    public SearchClassTypeSpecification(ClassTypesWithPaginationQuery query)
    {
        Criteria = q => q.Name != null;
        if (!string.IsNullOrEmpty(query.Keyword))
        {
            And(x => x.Name!.Contains(query.Keyword) || x.Description!.Contains(query.Keyword) || x.Code!.Contains(query.Keyword));
        }
        if (!string.IsNullOrEmpty(query.Name))
        {
            And(x => x.Name!.Contains(query.Name));
        }
    }
}

using MediatR;
using User.Authentication.Core.Application.Common.ViewModels;

namespace User.Authentication.Core.Application.Roles.Queries.GetRoles
{
    public record GetRolesQuery() : IRequest<IEnumerable<RoleVM>>;    
}

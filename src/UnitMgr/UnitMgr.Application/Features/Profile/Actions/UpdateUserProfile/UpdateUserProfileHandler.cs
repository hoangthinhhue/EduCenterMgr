using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorState;
using MediatR;
using Mgr.Core.Models;

namespace UnitMgr.Application.Features.Identity.Profile;

public partial class UserProfileState
{
    public class UpdateUserProfileHandler : ActionHandler<UpdateUserProfileAction>
    {
        public UpdateUserProfileHandler(IStore aStore) : base(aStore) { }

        UserProfileState UserProfileState => Store.GetState<UserProfileState>();

        public override Task<Unit> Handle(UpdateUserProfileAction updateAction, CancellationToken aCancellationToken)
        {

            UserProfileState.UserProfile = updateAction.UserProfile;
            return Unit.Task;
        }
        
    }

    public class UpdateUserDtoHandler : ActionHandler<UpdateUserDtoAction>
    {
        public UpdateUserDtoHandler(IStore aStore) : base(aStore) { }
        UserProfileState UserProfileState => Store.GetState<UserProfileState>();
        public override Task<Unit> Handle(UpdateUserDtoAction updateAction, CancellationToken aCancellationToken)
        {
            var dto = updateAction.UserDto;
            UserProfileState.UserProfile = new UserProfile()
            {
                UserId = dto.Id.ToString(),
                ProfilePictureDataUrl = dto.ProfilePictureDataUrl,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DisplayName = dto.DisplayName,
                Provider = dto.Provider,
                UserName = dto.UserName,
                IsActive = dto.IsActive,
                TenantId = dto.TenantId.ToString(),
                TenantName = dto.TenantName,
                AssignRoles = dto.AssignedRoles,
                Role = dto.Role
            };
            return Unit.Task;
        }
    }
}

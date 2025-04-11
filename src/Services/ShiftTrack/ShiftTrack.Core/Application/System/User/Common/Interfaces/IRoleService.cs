﻿using ShiftTrack.Core.Domain.System.User.Roles.Entities;

namespace ShiftTrack.Core.Application.System.User.Common.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<Role>> GetRoles(CancellationToken cancellationToken);
}
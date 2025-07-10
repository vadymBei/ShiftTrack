using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Domain.Features.System.Auth.Models;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;

namespace ShiftTrack.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    public Employee Employee { get; set; }
    public List<string> Roles { get; }
}
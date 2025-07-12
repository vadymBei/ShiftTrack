using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace User.Authentication.Application.Features.oAuth.Common.ViewModels;

[AutoMap(typeof(IdentityRole))]
public class RoleVm
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
}
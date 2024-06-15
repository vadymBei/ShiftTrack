using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace User.Authentication.Core.Application.Common.ViewModels
{
    [AutoMap(typeof(IdentityRole))]
    public class RoleVM
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }
    }
}

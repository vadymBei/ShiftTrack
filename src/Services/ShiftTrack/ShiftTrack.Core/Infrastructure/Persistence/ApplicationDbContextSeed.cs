using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.System.User.Common.Constants;
using ShiftTrack.Core.Domain.System.User.Roles.Entities;

namespace ShiftTrack.Core.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedDefaultRolesAsync(context);
    }

    private static async Task SeedDefaultRolesAsync(ApplicationDbContext context)
    {
        var rolesClass = typeof(DefaultRolesDirectory);
        var rolesFields = rolesClass.GetFields(BindingFlags.Public | BindingFlags.Static);

        var exitedRoles = await context.Roles
            .ToListAsync(CancellationToken.None);

        var newRoles = new List<Role>();

        foreach (var rolesField in rolesFields)
        {
            var roleName = rolesField.Name
                .ToLower()
                .Replace("_", ".");

            var roleTitle = $"[{roleName}] - {rolesField.GetValue(null)}";

            var localRole = exitedRoles
                .FirstOrDefault(x => x.Name == roleName);

            if (localRole is null)
            {
                newRoles.Add(new Role()
                {
                    Name = roleName,
                    Title = roleTitle
                });
            }
            else
            {
                localRole.Title = roleTitle;
                context.Roles.Update(localRole);
            }
        }

        if (newRoles.Any())
        {
            context.Roles.AddRange(newRoles);
        }

        await context.SaveChangesAsync(CancellationToken.None);
    }
}
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.System.User.Common.Constants;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Enums;
using ShiftTrack.Core.Domain.System.User.Roles.Entities;

namespace ShiftTrack.Core.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedDefaultRolesAsync(context);

        await SeedDefaultShiftsAsync(context);

        await SeedDefaultUnitsAsync(context);

        await SeedDefaultDepartmentsAsync(context);

        await SeedDefaultPositionsAsync(context);
    }

    private static async Task SeedDefaultRolesAsync(ApplicationDbContext context)
    {
        var rolesClass = typeof(DefaultRolesDirectory);
        var rolesFields = rolesClass.GetFields(BindingFlags.Public | BindingFlags.Static);

        var exitedRoles = await context.Roles
            .AsNoTracking()
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

    private static async Task SeedDefaultShiftsAsync(ApplicationDbContext context)
    {
        var shifts = new List<Shift>
        {
            new()
            {
                Code = "-",
                Description = "Звільнено",
                Color = "#FFFFFF",
                StartTime = null,
                EndTime = null,
                WorkHours = null,
                Type = ShiftType.None
            },
            new()
            {
                Code = "В",
                Description = "Основна щорічна відпустка",
                Color = "#FFF176",
                StartTime = null,
                EndTime = null,
                WorkHours = null,
                Type = ShiftType.Vacation
            },
            new()
            {
                Code = "ВД",
                Description = "Відрядження",
                Color = "#A08780",
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(20, 0, 0),
                WorkHours = new TimeSpan(10, 0, 0),
                Type = ShiftType.Workday
            },
            new()
            {
                Code = "ВП",
                Description = "Відпустка у зв'язку з вагітністю і пологами",
                Color = "#FFFFFF",
                StartTime = null,
                EndTime = null,
                WorkHours = null,
                Type = ShiftType.Vacation
            },
            new()
            {
                Code = "ВХ",
                Description = "Вихідний день",
                Color = "#E0E0E0",
                StartTime = null,
                EndTime = null,
                WorkHours = null,
                Type = ShiftType.DayOff
            },
            new()
            {
                Code = "ДД",
                Description = "Відпустка за дитиною до 6-ти років",
                Color = "#FFFFFF",
                StartTime = null,
                EndTime = null,
                WorkHours = null,
                Type = ShiftType.Vacation
            },
            new()
            {
                Code = "І",
                Description = "Інші причини неявок",
                Color = "#FFFFFF",
                StartTime = null,
                EndTime = null,
                WorkHours = null,
                Type = ShiftType.None
            },
            new()
            {
                Code = "НА",
                Description = "Відпустка без збереження заробітної плати за згодою обох сторін",
                Color = "#FFFFFF",
                StartTime = null,
                EndTime = null,
                WorkHours = null,
                Type = ShiftType.Vacation
            },
            new()
            {
                Code = "Р10",
                Description = "10-ти годинний робочий день",
                Color = "#DDE776",
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(20, 0, 0),
                WorkHours = new TimeSpan(10, 0, 0),
                Type = ShiftType.Workday
            },
            new()
            {
                Code = "Р9",
                Description = "9-ти годинний робочий день",
                Color = "#AED584",
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(19, 0, 0),
                WorkHours = new TimeSpan(9, 0, 0),
                Type = ShiftType.Workday
            }
        };


        var exitedShifts = await context.Shifts
            .ToListAsync(CancellationToken.None);

        var newShifts = (
                from shift in shifts
                let localShift = exitedShifts.FirstOrDefault(x => x.Code == shift.Code)
                where localShift is null
                select shift)
            .ToList();

        if (newShifts.Any())
        {
            context.Shifts.AddRange(newShifts);
            await context.SaveChangesAsync(CancellationToken.None);
        }
    }

    private static async Task SeedDefaultUnitsAsync(ApplicationDbContext context)
    {
        var units = new List<Unit>()
        {
            new()
            {
                Code = "KHML",
                Name = "Хмельницький",
                Description = "Хмельницький регіон"
            }
        };

        var exitedUnits = await context.Units
            .AsNoTracking()
            .ToListAsync(CancellationToken.None);

        var newUnits = (
                from unit in units
                let localUnit = exitedUnits.FirstOrDefault(x => x.Code == unit.Code)
                where localUnit is null
                select unit)
            .ToList();

        if (newUnits.Any())
        {
            context.Units.AddRange(newUnits);
            await context.SaveChangesAsync(CancellationToken.None);
        }
    }

    private static async Task SeedDefaultDepartmentsAsync(ApplicationDbContext context)
    {
        await SeedDefaultUnitsAsync(context);

        var unit = context.Units
            .AsNoTracking()
            .FirstOrDefault(x => x.Name == "Хмельницький");

        if (unit is not null)
        {
            var department = new Department()
            {
                Name = "ТРЦ Либідь Плаза",
                UnitId = unit.Id
            };

            var existedDepartments = await context.Departments
                .AsNoTracking()
                .ToListAsync(CancellationToken.None);

            var existedDepartment = existedDepartments
                .FirstOrDefault(x => x.Name == department.Name);

            if (existedDepartment is null)
            {
                context.Departments.Add(department);
                await context.SaveChangesAsync(CancellationToken.None);
            }
        }
    }

    private static async Task SeedDefaultPositionsAsync(ApplicationDbContext context)
    {
        var positions = new List<Position>()
        {
            new()
            {
                Name = "Адміністратор",
                Description = "Адміністратор магазину"
            },
            new()
            {
                Name = "Директор",
                Description = "Директор магазину"
            },
            new()
            {
                Name = "Регіональний директор",
                Description = "Регіональний директор"
            },
            new()
            {
                Name = "Стиліст",
                Description = "Стиліст магазину"
            },
        };

        var exitedPositions = await context.Positions
            .AsNoTracking()
            .ToListAsync(CancellationToken.None);

        var newPositions = (
                from position in positions
                let localPosition = exitedPositions.FirstOrDefault(x => x.Name == position.Name)
                where localPosition is null
                select position)
            .ToList();

        if (newPositions.Any())
        {
            context.Positions.AddRange(newPositions);
            await context.SaveChangesAsync(CancellationToken.None);
        }
    }
}
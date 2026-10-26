using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using ShiftTrack.Infrastructure.Persistence;

#nullable disable

namespace ShiftTrack.Infrastructure.Persistence.Migrations
{
    /// <summary>
    /// Role bindings became the only source of what a director manages. Directors that had no bindings
    /// managed their own department / unit, so the same scope is written as bindings:
    /// a regional director gets the unit of their department, a store director the unit with their department.
    /// Department directors bound to a unit without departments get their own department in it,
    /// otherwise such a binding would now cover the whole unit. No schema changes.
    /// </summary>
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20261026120000_Backfill_DirectorRoleBindings")]
    public class Backfill_DirectorRoleBindings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // RoleScope: Local = 1 (only listed units / departments), Global = 2 (everything / the whole unit).
            migrationBuilder.Sql("""
                INSERT INTO "EmployeeRoleUnits" ("Scope", "EmployeeRoleId", "UnitId", "CreatedAt")
                SELECT 2, er."Id", d."UnitId", now() AT TIME ZONE 'utc'
                FROM "EmployeeRoles" er
                JOIN "Roles" r ON r."Id" = er."RoleId"
                JOIN "Employees" e ON e."Id" = er."EmployeeId"
                JOIN "Departments" d ON d."Id" = e."DepartmentId"
                WHERE r."Name" = 'unit.director'
                  AND d."UnitId" IS NOT NULL
                  AND NOT EXISTS (SELECT 1 FROM "EmployeeRoleUnits" eru WHERE eru."EmployeeRoleId" = er."Id");
                """);

            migrationBuilder.Sql("""
                WITH inserted AS (
                    INSERT INTO "EmployeeRoleUnits" ("Scope", "EmployeeRoleId", "UnitId", "CreatedAt")
                    SELECT 1, er."Id", d."UnitId", now() AT TIME ZONE 'utc'
                    FROM "EmployeeRoles" er
                    JOIN "Roles" r ON r."Id" = er."RoleId"
                    JOIN "Employees" e ON e."Id" = er."EmployeeId"
                    JOIN "Departments" d ON d."Id" = e."DepartmentId"
                    WHERE r."Name" = 'department.director'
                      AND d."UnitId" IS NOT NULL
                      AND NOT EXISTS (SELECT 1 FROM "EmployeeRoleUnits" eru WHERE eru."EmployeeRoleId" = er."Id")
                    RETURNING "Id", "EmployeeRoleId"
                )
                INSERT INTO "EmployeeRoleUnitDepartments" ("EmployeeRoleUnitId", "DepartmentId", "CreatedAt")
                SELECT i."Id", e."DepartmentId", now() AT TIME ZONE 'utc'
                FROM inserted i
                JOIN "EmployeeRoles" er ON er."Id" = i."EmployeeRoleId"
                JOIN "Employees" e ON e."Id" = er."EmployeeId";
                """);

            migrationBuilder.Sql("""
                INSERT INTO "EmployeeRoleUnitDepartments" ("EmployeeRoleUnitId", "DepartmentId", "CreatedAt")
                SELECT eru."Id", e."DepartmentId", now() AT TIME ZONE 'utc'
                FROM "EmployeeRoleUnits" eru
                JOIN "EmployeeRoles" er ON er."Id" = eru."EmployeeRoleId"
                JOIN "Roles" r ON r."Id" = er."RoleId"
                JOIN "Employees" e ON e."Id" = er."EmployeeId"
                JOIN "Departments" d ON d."Id" = e."DepartmentId"
                WHERE r."Name" = 'department.director'
                  AND d."UnitId" = eru."UnitId"
                  AND NOT EXISTS (SELECT 1 FROM "EmployeeRoleUnitDepartments" erud WHERE erud."EmployeeRoleUnitId" = eru."Id");
                """);

            migrationBuilder.Sql("""
                UPDATE "EmployeeRoleUnits" eru
                SET "Scope" = CASE WHEN EXISTS (SELECT 1 FROM "EmployeeRoleUnitDepartments" erud
                                                WHERE erud."EmployeeRoleUnitId" = eru."Id") THEN 1 ELSE 2 END;

                UPDATE "EmployeeRoles" er
                SET "Scope" = CASE WHEN EXISTS (SELECT 1 FROM "EmployeeRoleUnits" eru
                                                WHERE eru."EmployeeRoleId" = er."Id") THEN 1 ELSE 2 END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Data only: the created bindings stay, they describe the same scope as before.
        }
    }
}

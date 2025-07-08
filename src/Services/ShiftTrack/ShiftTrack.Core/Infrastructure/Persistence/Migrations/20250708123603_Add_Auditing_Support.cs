using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftTrack.Core.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Auditing_Support : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Units",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Units",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Units",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "Units",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Shifts",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Shifts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Shifts",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "Shifts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Roles",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Roles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Roles",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "Roles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Positions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Positions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Positions",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "Positions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Employees",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Employees",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Employees",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "Employees",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EmployeeRoleUnits",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "EmployeeRoleUnits",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "EmployeeRoleUnits",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "EmployeeRoleUnits",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EmployeeRoleUnitDepartments",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "EmployeeRoleUnitDepartments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "EmployeeRoleUnitDepartments",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "EmployeeRoleUnitDepartments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EmployeeRoles",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "EmployeeRoles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "EmployeeRoles",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "EmployeeRoles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Departments",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Departments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Departments",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "Departments",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EmployeeRoleUnits");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "EmployeeRoleUnits");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "EmployeeRoleUnits");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "EmployeeRoleUnits");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EmployeeRoleUnitDepartments");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "EmployeeRoleUnitDepartments");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "EmployeeRoleUnitDepartments");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "EmployeeRoleUnitDepartments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EmployeeRoles");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "EmployeeRoles");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "EmployeeRoles");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "EmployeeRoles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Departments");
        }
    }
}

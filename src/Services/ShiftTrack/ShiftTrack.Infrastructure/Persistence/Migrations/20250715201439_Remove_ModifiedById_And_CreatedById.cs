using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftTrack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Remove_ModifiedById_And_CreatedById : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "EmployeeRoleUnits");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "EmployeeRoleUnits");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "EmployeeRoleUnitDepartments");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "EmployeeRoleUnitDepartments");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "EmployeeRoles");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "EmployeeRoles");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Departments");

            migrationBuilder.RenameColumn(
                name: "ModifiedById",
                table: "Vacations",
                newName: "ModifierId");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Vacations",
                newName: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacations_AuthorId",
                table: "Vacations",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacations_ModifierId",
                table: "Vacations",
                column: "ModifierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacations_Employees_AuthorId",
                table: "Vacations",
                column: "AuthorId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacations_Employees_ModifierId",
                table: "Vacations",
                column: "ModifierId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacations_Employees_AuthorId",
                table: "Vacations");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacations_Employees_ModifierId",
                table: "Vacations");

            migrationBuilder.DropIndex(
                name: "IX_Vacations_AuthorId",
                table: "Vacations");

            migrationBuilder.DropIndex(
                name: "IX_Vacations_ModifierId",
                table: "Vacations");

            migrationBuilder.RenameColumn(
                name: "ModifierId",
                table: "Vacations",
                newName: "ModifiedById");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Vacations",
                newName: "CreatedById");

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Units",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "Units",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Shifts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "Shifts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Roles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "Roles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Positions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "Positions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Employees",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "Employees",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "EmployeeRoleUnits",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "EmployeeRoleUnits",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "EmployeeRoleUnitDepartments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "EmployeeRoleUnitDepartments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "EmployeeRoles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "EmployeeRoles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreatedById",
                table: "Departments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifiedById",
                table: "Departments",
                type: "bigint",
                nullable: true);
        }
    }
}

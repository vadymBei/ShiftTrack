using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftTrack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_AuthorId_And_ModifierId_To_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacations_Employees_AuthorId",
                table: "Vacations");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacations_Employees_ModifierId",
                table: "Vacations");

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "Units",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifierId",
                table: "Units",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "Shifts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifierId",
                table: "Shifts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "Roles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifierId",
                table: "Roles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "Positions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifierId",
                table: "Positions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "Employees",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifierId",
                table: "Employees",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "EmployeeRoleUnits",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifierId",
                table: "EmployeeRoleUnits",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "EmployeeRoleUnitDepartments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifierId",
                table: "EmployeeRoleUnitDepartments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "EmployeeRoles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifierId",
                table: "EmployeeRoles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "Departments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ModifierId",
                table: "Departments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_AuthorId",
                table: "Units",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_ModifierId",
                table: "Units",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_AuthorId",
                table: "Shifts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_ModifierId",
                table: "Shifts",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_AuthorId",
                table: "Roles",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ModifierId",
                table: "Roles",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_AuthorId",
                table: "Positions",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_ModifierId",
                table: "Positions",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AuthorId",
                table: "Employees",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ModifierId",
                table: "Employees",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoleUnits_AuthorId",
                table: "EmployeeRoleUnits",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoleUnits_ModifierId",
                table: "EmployeeRoleUnits",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoleUnitDepartments_AuthorId",
                table: "EmployeeRoleUnitDepartments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoleUnitDepartments_ModifierId",
                table: "EmployeeRoleUnitDepartments",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoles_AuthorId",
                table: "EmployeeRoles",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoles_ModifierId",
                table: "EmployeeRoles",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_AuthorId",
                table: "Departments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ModifierId",
                table: "Departments",
                column: "ModifierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employees_AuthorId",
                table: "Departments",
                column: "AuthorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employees_ModifierId",
                table: "Departments",
                column: "ModifierId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRoles_Employees_AuthorId",
                table: "EmployeeRoles",
                column: "AuthorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRoles_Employees_ModifierId",
                table: "EmployeeRoles",
                column: "ModifierId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRoleUnitDepartments_Employees_AuthorId",
                table: "EmployeeRoleUnitDepartments",
                column: "AuthorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRoleUnitDepartments_Employees_ModifierId",
                table: "EmployeeRoleUnitDepartments",
                column: "ModifierId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRoleUnits_Employees_AuthorId",
                table: "EmployeeRoleUnits",
                column: "AuthorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRoleUnits_Employees_ModifierId",
                table: "EmployeeRoleUnits",
                column: "ModifierId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_AuthorId",
                table: "Employees",
                column: "AuthorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_ModifierId",
                table: "Employees",
                column: "ModifierId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Employees_AuthorId",
                table: "Positions",
                column: "AuthorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Employees_ModifierId",
                table: "Positions",
                column: "ModifierId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Employees_AuthorId",
                table: "Roles",
                column: "AuthorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Employees_ModifierId",
                table: "Roles",
                column: "ModifierId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Employees_AuthorId",
                table: "Shifts",
                column: "AuthorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Employees_ModifierId",
                table: "Shifts",
                column: "ModifierId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Employees_AuthorId",
                table: "Units",
                column: "AuthorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Employees_ModifierId",
                table: "Units",
                column: "ModifierId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacations_Employees_AuthorId",
                table: "Vacations",
                column: "AuthorId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacations_Employees_ModifierId",
                table: "Vacations",
                column: "ModifierId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employees_AuthorId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employees_ModifierId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRoles_Employees_AuthorId",
                table: "EmployeeRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRoles_Employees_ModifierId",
                table: "EmployeeRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRoleUnitDepartments_Employees_AuthorId",
                table: "EmployeeRoleUnitDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRoleUnitDepartments_Employees_ModifierId",
                table: "EmployeeRoleUnitDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRoleUnits_Employees_AuthorId",
                table: "EmployeeRoleUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRoleUnits_Employees_ModifierId",
                table: "EmployeeRoleUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_AuthorId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_ModifierId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Employees_AuthorId",
                table: "Positions");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Employees_ModifierId",
                table: "Positions");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Employees_AuthorId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Employees_ModifierId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Employees_AuthorId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Employees_ModifierId",
                table: "Shifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Employees_AuthorId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Employees_ModifierId",
                table: "Units");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacations_Employees_AuthorId",
                table: "Vacations");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacations_Employees_ModifierId",
                table: "Vacations");

            migrationBuilder.DropIndex(
                name: "IX_Units_AuthorId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Units_ModifierId",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_AuthorId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_ModifierId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Roles_AuthorId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_ModifierId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Positions_AuthorId",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Positions_ModifierId",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Employees_AuthorId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ModifierId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeRoleUnits_AuthorId",
                table: "EmployeeRoleUnits");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeRoleUnits_ModifierId",
                table: "EmployeeRoleUnits");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeRoleUnitDepartments_AuthorId",
                table: "EmployeeRoleUnitDepartments");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeRoleUnitDepartments_ModifierId",
                table: "EmployeeRoleUnitDepartments");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeRoles_AuthorId",
                table: "EmployeeRoles");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeRoles_ModifierId",
                table: "EmployeeRoles");

            migrationBuilder.DropIndex(
                name: "IX_Departments_AuthorId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_ModifierId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "EmployeeRoleUnits");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "EmployeeRoleUnits");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "EmployeeRoleUnitDepartments");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "EmployeeRoleUnitDepartments");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "EmployeeRoles");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "EmployeeRoles");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "Departments");

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
    }
}

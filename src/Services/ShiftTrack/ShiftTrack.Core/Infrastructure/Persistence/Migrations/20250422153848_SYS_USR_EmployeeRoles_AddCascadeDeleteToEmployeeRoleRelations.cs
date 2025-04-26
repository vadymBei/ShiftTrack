using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftTrack.Core.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SYS_USR_EmployeeRoles_AddCascadeDeleteToEmployeeRoleRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRoleUnitDepartments_EmployeeRoleUnits_EmployeeRoleU~",
                table: "EmployeeRoleUnitDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRoleUnits_EmployeeRoles_EmployeeRoleId",
                table: "EmployeeRoleUnits");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRoleUnitDepartments_EmployeeRoleUnits_EmployeeRoleU~",
                table: "EmployeeRoleUnitDepartments",
                column: "EmployeeRoleUnitId",
                principalTable: "EmployeeRoleUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRoleUnits_EmployeeRoles_EmployeeRoleId",
                table: "EmployeeRoleUnits",
                column: "EmployeeRoleId",
                principalTable: "EmployeeRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRoleUnitDepartments_EmployeeRoleUnits_EmployeeRoleU~",
                table: "EmployeeRoleUnitDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRoleUnits_EmployeeRoles_EmployeeRoleId",
                table: "EmployeeRoleUnits");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRoleUnitDepartments_EmployeeRoleUnits_EmployeeRoleU~",
                table: "EmployeeRoleUnitDepartments",
                column: "EmployeeRoleUnitId",
                principalTable: "EmployeeRoleUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRoleUnits_EmployeeRoles_EmployeeRoleId",
                table: "EmployeeRoleUnits",
                column: "EmployeeRoleId",
                principalTable: "EmployeeRoles",
                principalColumn: "Id");
        }
    }
}

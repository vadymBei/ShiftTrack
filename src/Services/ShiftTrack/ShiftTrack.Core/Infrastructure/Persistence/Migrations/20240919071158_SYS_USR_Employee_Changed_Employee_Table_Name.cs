using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftTrack.Core.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SYS_USR_Employee_Changed_Employee_Table_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Departments_DepartmentId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Positions_PositionId",
                table: "Profiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Profiles",
                table: "Profiles");

            migrationBuilder.RenameTable(
                name: "Profiles",
                newName: "Employees");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_PositionId",
                table: "Employees",
                newName: "IX_Employees_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_PhoneNumber",
                table: "Employees",
                newName: "IX_Employees_PhoneNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_IsDeleted",
                table: "Employees",
                newName: "IX_Employees_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_Email",
                table: "Employees",
                newName: "IX_Employees_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_DepartmentId",
                table: "Employees",
                newName: "IX_Employees_DepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Positions_PositionId",
                table: "Employees",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Positions_PositionId",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Profiles");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_PositionId",
                table: "Profiles",
                newName: "IX_Profiles_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_PhoneNumber",
                table: "Profiles",
                newName: "IX_Profiles_PhoneNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_IsDeleted",
                table: "Profiles",
                newName: "IX_Profiles_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_Email",
                table: "Profiles",
                newName: "IX_Profiles_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_DepartmentId",
                table: "Profiles",
                newName: "IX_Profiles_DepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Profiles",
                table: "Profiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Departments_DepartmentId",
                table: "Profiles",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Positions_PositionId",
                table: "Profiles",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id");
        }
    }
}

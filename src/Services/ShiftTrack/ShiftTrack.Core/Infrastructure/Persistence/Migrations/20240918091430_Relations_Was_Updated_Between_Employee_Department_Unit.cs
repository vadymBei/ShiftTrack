using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftTrack.Core.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Relations_Was_Updated_Between_Employee_Department_Unit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Units_UnitId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_UnitId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Profiles");

            migrationBuilder.AddColumn<long>(
                name: "UnitId",
                table: "Departments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_UnitId",
                table: "Departments",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Units_UnitId",
                table: "Departments",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Units_UnitId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_UnitId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Departments");

            migrationBuilder.AddColumn<long>(
                name: "UnitId",
                table: "Profiles",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UnitId",
                table: "Profiles",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Units_UnitId",
                table: "Profiles",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }
    }
}

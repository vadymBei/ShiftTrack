using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftTrack.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Department_UnitId_Prop_Was_Removed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}

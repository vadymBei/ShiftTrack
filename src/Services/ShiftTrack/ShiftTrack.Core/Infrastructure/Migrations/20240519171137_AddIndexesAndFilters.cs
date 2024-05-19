using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftTrack.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesAndFilters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Units_IsDeleted",
                table: "Units",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_IsDeleted",
                table: "Shifts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_IsDeleted",
                table: "Positions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_IsDeleted",
                table: "Departments",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Units_IsDeleted",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_IsDeleted",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Positions_IsDeleted",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Departments_IsDeleted",
                table: "Departments");
        }
    }
}

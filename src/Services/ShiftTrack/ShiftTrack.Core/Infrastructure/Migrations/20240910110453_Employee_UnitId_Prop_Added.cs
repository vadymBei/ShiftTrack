using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftTrack.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Employee_UnitId_Prop_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftTrack.Core.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Employee_Added_PositionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PositionId",
                table: "Profiles",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_PositionId",
                table: "Profiles",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Positions_PositionId",
                table: "Profiles",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Positions_PositionId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_PositionId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "Profiles");
        }
    }
}

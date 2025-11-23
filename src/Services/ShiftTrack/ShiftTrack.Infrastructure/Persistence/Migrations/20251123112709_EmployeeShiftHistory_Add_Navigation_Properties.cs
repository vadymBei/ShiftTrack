using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftTrack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeShiftHistory_Add_Navigation_Properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftHistory_NewShiftId",
                table: "EmployeeShiftHistory",
                column: "NewShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftHistory_PreviousShiftId",
                table: "EmployeeShiftHistory",
                column: "PreviousShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeShiftHistory_Shifts_NewShiftId",
                table: "EmployeeShiftHistory",
                column: "NewShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeShiftHistory_Shifts_PreviousShiftId",
                table: "EmployeeShiftHistory",
                column: "PreviousShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeShiftHistory_Shifts_NewShiftId",
                table: "EmployeeShiftHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeShiftHistory_Shifts_PreviousShiftId",
                table: "EmployeeShiftHistory");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeShiftHistory_NewShiftId",
                table: "EmployeeShiftHistory");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeShiftHistory_PreviousShiftId",
                table: "EmployeeShiftHistory");
        }
    }
}

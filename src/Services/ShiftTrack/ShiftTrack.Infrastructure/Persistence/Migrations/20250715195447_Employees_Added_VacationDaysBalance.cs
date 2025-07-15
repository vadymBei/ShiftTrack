using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftTrack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Employees_Added_VacationDaysBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VacationDaysBalance",
                table: "Employees",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VacationDaysBalance",
                table: "Employees");
        }
    }
}

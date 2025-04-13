using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftTrack.Core.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ORG_TSH_Shifts_Rename_Description : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Dercription",
                table: "Shifts",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Shifts",
                newName: "Dercription");
        }
    }
}

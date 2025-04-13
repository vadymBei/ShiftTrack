using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftTrack.Core.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ORG_TSH_Shifts_Added_StartTime_EndTime_WorkHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "Shifts",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "Shifts",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkHours",
                table: "Shifts",
                type: "interval",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "WorkHours",
                table: "Shifts");
        }
    }
}

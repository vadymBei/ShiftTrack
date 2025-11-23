using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShiftTrack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_EmployeeShiftHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeShiftHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeShiftId = table.Column<long>(type: "bigint", nullable: false),
                    PreviousShiftId = table.Column<long>(type: "bigint", nullable: true),
                    PreviousStartTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    PreviousEndTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    NewShiftId = table.Column<long>(type: "bigint", nullable: true),
                    NewStartTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    NewEndTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AuthorId = table.Column<long>(type: "bigint", nullable: true),
                    ModifierId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeShiftHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeShiftHistory_EmployeeShifts_EmployeeShiftId",
                        column: x => x.EmployeeShiftId,
                        principalTable: "EmployeeShifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeShiftHistory_Employees_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeShiftHistory_Employees_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftHistory_AuthorId",
                table: "EmployeeShiftHistory",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftHistory_EmployeeShiftId",
                table: "EmployeeShiftHistory",
                column: "EmployeeShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShiftHistory_ModifierId",
                table: "EmployeeShiftHistory",
                column: "ModifierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeShiftHistory");
        }
    }
}

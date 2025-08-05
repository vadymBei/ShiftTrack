using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShiftTrack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_BusinessTrip_And_BusinessTripParticipant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessTrips",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Route = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    EstimatedBudget = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AuthorId = table.Column<long>(type: "bigint", nullable: true),
                    ModifierId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessTrips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessTrips_Employees_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BusinessTrips_Employees_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BusinessTripParticipants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessTripId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AuthorId = table.Column<long>(type: "bigint", nullable: true),
                    ModifierId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessTripParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessTripParticipants_BusinessTrips_BusinessTripId",
                        column: x => x.BusinessTripId,
                        principalTable: "BusinessTrips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessTripParticipants_Employees_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BusinessTripParticipants_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessTripParticipants_Employees_ModifierId",
                        column: x => x.ModifierId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTripParticipants_AuthorId",
                table: "BusinessTripParticipants",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTripParticipants_BusinessTripId",
                table: "BusinessTripParticipants",
                column: "BusinessTripId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTripParticipants_EmployeeId",
                table: "BusinessTripParticipants",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTripParticipants_ModifierId",
                table: "BusinessTripParticipants",
                column: "ModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTrips_AuthorId",
                table: "BusinessTrips",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTrips_ModifierId",
                table: "BusinessTrips",
                column: "ModifierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessTripParticipants");

            migrationBuilder.DropTable(
                name: "BusinessTrips");
        }
    }
}

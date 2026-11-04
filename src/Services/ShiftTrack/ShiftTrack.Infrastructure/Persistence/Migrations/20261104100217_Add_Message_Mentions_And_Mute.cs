using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftTrack.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Message_Mentions_And_Mute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMuted",
                table: "ConversationMembers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MessageMentions",
                columns: table => new
                {
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageMentions", x => new { x.MessageId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_MessageMentions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageMentions_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageMentions_EmployeeId_MessageId",
                table: "MessageMentions",
                columns: new[] { "EmployeeId", "MessageId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageMentions");

            migrationBuilder.DropColumn(
                name: "IsMuted",
                table: "ConversationMembers");
        }
    }
}

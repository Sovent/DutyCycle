using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DutyCycle.API.Migrations
{
    public partial class AddTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupActionTrigger",
                columns: table => new
                {
                    Action = table.Column<int>(type: "integer", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupActionTrigger", x => new { x.GroupId, x.Action });
                    table.ForeignKey(
                        name: "FK_GroupActionTrigger_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TriggerCallback",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CallbackType = table.Column<string>(type: "text", nullable: false),
                    GroupActionTriggerAction = table.Column<int>(type: "integer", nullable: true),
                    GroupActionTriggerGroupId = table.Column<int>(type: "integer", nullable: true),
                    ChannelId = table.Column<string>(type: "text", nullable: true),
                    MessageTextTemplate = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriggerCallback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TriggerCallback_GroupActionTrigger_GroupActionTriggerGroupI~",
                        columns: x => new { x.GroupActionTriggerGroupId, x.GroupActionTriggerAction },
                        principalTable: "GroupActionTrigger",
                        principalColumns: new[] { "GroupId", "Action" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TriggerCallback_GroupActionTriggerGroupId_GroupActionTrigge~",
                table: "TriggerCallback",
                columns: new[] { "GroupActionTriggerGroupId", "GroupActionTriggerAction" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TriggerCallback");

            migrationBuilder.DropTable(
                name: "GroupActionTrigger");
        }
    }
}

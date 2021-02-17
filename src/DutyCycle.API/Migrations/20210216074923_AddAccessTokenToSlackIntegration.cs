using Microsoft.EntityFrameworkCore.Migrations;

namespace DutyCycle.API.Migrations
{
    public partial class AddAccessTokenToSlackIntegration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InitiatedSlackConnections_Organizations_OrganizationId",
                table: "InitiatedSlackConnections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InitiatedSlackConnections",
                table: "InitiatedSlackConnections");

            migrationBuilder.RenameTable(
                name: "InitiatedSlackConnections",
                newName: "SlackConnections");

            migrationBuilder.RenameIndex(
                name: "IX_InitiatedSlackConnections_OrganizationId",
                table: "SlackConnections",
                newName: "IX_SlackConnections_OrganizationId");

            migrationBuilder.AddColumn<string>(
                name: "_accessToken",
                table: "SlackConnections",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SlackConnections",
                table: "SlackConnections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SlackConnections_Organizations_OrganizationId",
                table: "SlackConnections",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SlackConnections_Organizations_OrganizationId",
                table: "SlackConnections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SlackConnections",
                table: "SlackConnections");

            migrationBuilder.DropColumn(
                name: "_accessToken",
                table: "SlackConnections");

            migrationBuilder.RenameTable(
                name: "SlackConnections",
                newName: "InitiatedSlackConnections");

            migrationBuilder.RenameIndex(
                name: "IX_SlackConnections_OrganizationId",
                table: "InitiatedSlackConnections",
                newName: "IX_InitiatedSlackConnections_OrganizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InitiatedSlackConnections",
                table: "InitiatedSlackConnections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InitiatedSlackConnections_Organizations_OrganizationId",
                table: "InitiatedSlackConnections",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DutyCycle.API.Migrations
{
    public partial class AddSlackIntegration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InitiatedSlackConnections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitiatedSlackConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InitiatedSlackConnections_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InitiatedSlackConnections_OrganizationId",
                table: "InitiatedSlackConnections",
                column: "OrganizationId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InitiatedSlackConnections");
        }
    }
}

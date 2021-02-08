using Microsoft.EntityFrameworkCore.Migrations;

namespace DutyCycle.API.Migrations
{
    public partial class AddOrganizationToGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"RotationChangedTrigger\"", true);
            migrationBuilder.Sql("DELETE FROM \"GroupMember\"", true);
            migrationBuilder.Sql("DELETE FROM \"Groups\"", true);
            
            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_OrganizationId",
                table: "Groups",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Organizations_OrganizationId",
                table: "Groups",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Organizations_OrganizationId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_OrganizationId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Groups");
        }
    }
}

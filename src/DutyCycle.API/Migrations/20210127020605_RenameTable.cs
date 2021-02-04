using Microsoft.EntityFrameworkCore.Migrations;

namespace DutyCycle.API.Migrations
{
    public partial class RenameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupActionTrigger_Groups_GroupId",
                table: "GroupActionTrigger");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupActionTrigger",
                table: "GroupActionTrigger");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "GroupActionTrigger");

            migrationBuilder.RenameTable(
                name: "GroupActionTrigger",
                newName: "RotationChangedTrigger");

            migrationBuilder.RenameIndex(
                name: "IX_GroupActionTrigger_GroupId",
                table: "RotationChangedTrigger",
                newName: "IX_RotationChangedTrigger_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RotationChangedTrigger",
                table: "RotationChangedTrigger",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RotationChangedTrigger_Groups_GroupId",
                table: "RotationChangedTrigger",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RotationChangedTrigger_Groups_GroupId",
                table: "RotationChangedTrigger");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RotationChangedTrigger",
                table: "RotationChangedTrigger");

            migrationBuilder.RenameTable(
                name: "RotationChangedTrigger",
                newName: "GroupActionTrigger");

            migrationBuilder.RenameIndex(
                name: "IX_RotationChangedTrigger_GroupId",
                table: "GroupActionTrigger",
                newName: "IX_GroupActionTrigger_GroupId");

            migrationBuilder.AddColumn<int>(
                name: "Action",
                table: "GroupActionTrigger",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupActionTrigger",
                table: "GroupActionTrigger",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupActionTrigger_Groups_GroupId",
                table: "GroupActionTrigger",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

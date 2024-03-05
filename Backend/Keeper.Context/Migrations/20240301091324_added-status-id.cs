using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Keeper.Context.Migrations
{
    public partial class addedstatusid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "Items",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_StatusId",
                table: "Items",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemStatus_StatusId",
                table: "Items",
                column: "StatusId",
                principalTable: "ItemStatus",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemStatus_StatusId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_StatusId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Items");
        }
    }
}

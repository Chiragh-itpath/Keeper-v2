using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Keeper.Context.Migrations
{
    public partial class contacttablechanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_Users_UserId",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Contact");

            migrationBuilder.RenameColumn(
                name: "Contact",
                table: "Users",
                newName: "Mobile");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Contact",
                newName: "AddedId");

            migrationBuilder.RenameIndex(
                name: "IX_Contact_UserId",
                table: "Contact",
                newName: "IX_Contact_AddedId");

            migrationBuilder.AddColumn<Guid>(
                name: "AddedById",
                table: "Contact",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Contact_AddedById",
                table: "Contact",
                column: "AddedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Users_AddedById",
                table: "Contact",
                column: "AddedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Users_AddedId",
                table: "Contact",
                column: "AddedId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_Users_AddedById",
                table: "Contact");

            migrationBuilder.DropForeignKey(
                name: "FK_Contact_Users_AddedId",
                table: "Contact");

            migrationBuilder.DropIndex(
                name: "IX_Contact_AddedById",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "AddedById",
                table: "Contact");

            migrationBuilder.RenameColumn(
                name: "Mobile",
                table: "Users",
                newName: "Contact");

            migrationBuilder.RenameColumn(
                name: "AddedId",
                table: "Contact",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Contact_AddedId",
                table: "Contact",
                newName: "IX_Contact_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Contact",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Users_UserId",
                table: "Contact",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}

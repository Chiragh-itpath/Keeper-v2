using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Keeper.Context.Migrations
{
    public partial class ContactTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Contact",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
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
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contact_Users_UserId",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Contact");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Contact");

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contact_Users_AddedId",
                table: "Contact",
                column: "AddedId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

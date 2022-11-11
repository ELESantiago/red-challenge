using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REDChallenge.Persistance.Migrations
{
    public partial class AuthRelated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "Id",
                keyValue: new Guid("ce2a654b-18b3-49af-90bd-de9f9d1443ab"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("cbdc660b-9bcc-4db8-a776-b3afa6dde0f3"));

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OrderType",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("95d97496-8a72-4f63-bfcd-75c8fca24c95"), "Test customer 1" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Name", "Password", "Salt" },
                values: new object[] { new Guid("b8007a51-e0f3-4b51-a1a2-757bb795248a"), "Test user 1", "", "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "Id",
                keyValue: new Guid("95d97496-8a72-4f63-bfcd-75c8fca24c95"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("b8007a51-e0f3-4b51-a1a2-757bb795248a"));

            migrationBuilder.DropColumn(
                name: "Password",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OrderType");

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[] { new Guid("ce2a654b-18b3-49af-90bd-de9f9d1443ab"), false, "Test customer 1" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[] { new Guid("cbdc660b-9bcc-4db8-a776-b3afa6dde0f3"), false, "Test user 1" });
        }
    }
}

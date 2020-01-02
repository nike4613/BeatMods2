using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeatMods2.Migrations
{
    public partial class NameIsIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_Name",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name")
                .Annotation("Npgsql:IndexInclude", new[] { "Id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Name",
                table: "Users");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_Name",
                table: "Users",
                column: "Name");
        }
    }
}

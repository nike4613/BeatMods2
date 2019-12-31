using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeatMods2.Migrations
{
    public partial class AddAuthCodeStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<string>>(
                name: "Permissions",
                table: "Groups",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AuthCodes",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Expires = table.Column<DateTime>(nullable: false),
                    GitHubBearer = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthCodes", x => x.Code);
                    table.UniqueConstraint("AK_AuthCodes_GitHubBearer", x => x.GitHubBearer);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthCodes");

            migrationBuilder.AlterColumn<List<string>>(
                name: "Permissions",
                table: "Groups",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(List<string>));
        }
    }
}

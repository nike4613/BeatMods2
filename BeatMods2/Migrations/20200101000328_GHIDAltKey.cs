using Microsoft.EntityFrameworkCore.Migrations;

namespace BeatMods2.Migrations
{
    public partial class GHIDAltKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_GithubToken",
                table: "Users");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_GithubId",
                table: "Users",
                column: "GithubId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_GithubId",
                table: "Users");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_GithubToken",
                table: "Users",
                column: "GithubToken");
        }
    }
}

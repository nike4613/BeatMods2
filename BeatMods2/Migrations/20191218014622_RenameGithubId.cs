using Microsoft.EntityFrameworkCore.Migrations;

namespace BeatMods2.Migrations
{
    public partial class RenameGithubId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_GithubId",
                table: "Users");

            migrationBuilder.RenameColumn("GithubId", "Users", "GithubToken");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_GithubToken",
                table: "Users",
                column: "GithubToken");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_GithubToken",
                table: "Users");

            migrationBuilder.RenameColumn("GithubToken", "Users", "GithubId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_GithubId",
                table: "Users",
                column: "GithubId");
        }
    }
}

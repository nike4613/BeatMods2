using Microsoft.EntityFrameworkCore.Migrations;

namespace BeatMods2.Migrations
{
    public partial class AddGithubId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GithubId",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GithubId",
                table: "Users");
        }
    }
}

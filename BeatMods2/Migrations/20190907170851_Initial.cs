using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeatMods2.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VersionName = table.Column<string>(nullable: false),
                    SteamBuildId = table.Column<string>(nullable: false),
                    Visibility = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameVersions", x => x.Id);
                    table.UniqueConstraint("AK_GameVersions_VersionName", x => x.VersionName);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Permissions = table.Column<List<string>>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.UniqueConstraint("AK_Groups_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.UniqueConstraint("AK_Tags_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Profile = table.Column<string>(nullable: false),
                    GithubId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_GithubId", x => x.GithubId);
                    table.UniqueConstraint("AK_Users_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "GameVersion_Group_Join",
                columns: table => new
                {
                    GameVersionId = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameVersion_Group_Join", x => new { x.GameVersionId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_GameVersion_Group_Join_GameVersions_GameVersionId",
                        column: x => x.GameVersionId,
                        principalTable: "GameVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameVersion_Group_Join_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mods",
                columns: table => new
                {
                    UUID = table.Column<Guid>(nullable: false),
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: false),
                    UploadedById = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Version = table.Column<string>(nullable: false),
                    Uploaded = table.Column<DateTime>(nullable: false),
                    Approved = table.Column<DateTime>(nullable: true),
                    ApprovalState = table.Column<string>(nullable: false),
                    GameVersionId = table.Column<Guid>(nullable: false),
                    System = table.Column<string>(nullable: false),
                    Required = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mods", x => x.UUID);
                    table.UniqueConstraint("AK_Mods_Id_Version", x => new { x.Id, x.Version });
                    table.ForeignKey(
                        name: "FK_Mods_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mods_GameVersions_GameVersionId",
                        column: x => x.GameVersionId,
                        principalTable: "GameVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mods_Users_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: false),
                    Body = table.Column<string>(nullable: false),
                    Posted = table.Column<DateTime>(nullable: false),
                    Edited = table.Column<DateTime>(nullable: true),
                    System = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User_Group_Join",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Group_Join", x => new { x.UserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_User_Group_Join_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Group_Join_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Download",
                columns: table => new
                {
                    Mod = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    CdnFile = table.Column<string>(nullable: false),
                    Hashes = table.Column<string>(nullable: false),
                    ModUUID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Download", x => new { x.Mod, x.Type });
                    table.ForeignKey(
                        name: "FK_Download_Mods_ModUUID",
                        column: x => x.ModUUID,
                        principalTable: "Mods",
                        principalColumn: "UUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mod_Tag_Join",
                columns: table => new
                {
                    ModId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mod_Tag_Join", x => new { x.ModId, x.TagId });
                    table.ForeignKey(
                        name: "FK_Mod_Tag_Join_Mods_ModId",
                        column: x => x.ModId,
                        principalTable: "Mods",
                        principalColumn: "UUID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mod_Tag_Join_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModRange",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    VersionRange = table.Column<string>(nullable: false),
                    ModUUID = table.Column<Guid>(nullable: true),
                    ModUUID1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModRange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModRange_Mods_ModUUID",
                        column: x => x.ModUUID,
                        principalTable: "Mods",
                        principalColumn: "UUID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModRange_Mods_ModUUID1",
                        column: x => x.ModUUID1,
                        principalTable: "Mods",
                        principalColumn: "UUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Download_ModUUID",
                table: "Download",
                column: "ModUUID");

            migrationBuilder.CreateIndex(
                name: "IX_GameVersion_Group_Join_GroupId",
                table: "GameVersion_Group_Join",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Mod_Tag_Join_TagId",
                table: "Mod_Tag_Join",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ModRange_ModUUID",
                table: "ModRange",
                column: "ModUUID");

            migrationBuilder.CreateIndex(
                name: "IX_ModRange_ModUUID1",
                table: "ModRange",
                column: "ModUUID1");

            migrationBuilder.CreateIndex(
                name: "IX_Mods_AuthorId",
                table: "Mods",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Mods_GameVersionId",
                table: "Mods",
                column: "GameVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Mods_UploadedById",
                table: "Mods",
                column: "UploadedById");

            migrationBuilder.CreateIndex(
                name: "IX_News_AuthorId",
                table: "News",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Group_Join_GroupId",
                table: "User_Group_Join",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Download");

            migrationBuilder.DropTable(
                name: "GameVersion_Group_Join");

            migrationBuilder.DropTable(
                name: "Mod_Tag_Join");

            migrationBuilder.DropTable(
                name: "ModRange");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "User_Group_Join");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Mods");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "GameVersions");
        }
    }
}

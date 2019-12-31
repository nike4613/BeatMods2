﻿// <auto-generated />
using System;
using System.Collections.Generic;
using BeatMods2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BeatMods2.Migrations
{
    [DbContext(typeof(ModRepoContext))]
    partial class ModRepoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("BeatMods2.Models.AuthCodeTempStore", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("GitHubBearer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.HasAlternateKey("GitHubBearer");

                    b.ToTable("AuthCodes");
                });

            modelBuilder.Entity("BeatMods2.Models.Download", b =>
                {
                    b.Property<Guid>("Mod")
                        .HasColumnType("uuid");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.Property<string>("CdnFile")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Dictionary<string, string>>("Hashes")
                        .IsRequired()
                        .HasColumnType("hstore");

                    b.Property<Guid?>("ModUUID")
                        .HasColumnType("uuid");

                    b.HasKey("Mod", "Type");

                    b.HasIndex("ModUUID");

                    b.ToTable("Download");
                });

            modelBuilder.Entity("BeatMods2.Models.GameVersion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("SteamBuildId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("VersionName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Visibility")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasAlternateKey("VersionName");

                    b.ToTable("GameVersions");
                });

            modelBuilder.Entity("BeatMods2.Models.GameVersion_Group_Join", b =>
                {
                    b.Property<Guid>("GameVersionId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.HasKey("GameVersionId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("GameVersion_Group_Join");
                });

            modelBuilder.Entity("BeatMods2.Models.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<string>>("Permissions")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("BeatMods2.Models.Mod", b =>
                {
                    b.Property<Guid>("UUID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ApprovalState")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("Approved")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("GameVersionId")
                        .HasColumnType("uuid");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("License")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Required")
                        .HasColumnType("boolean");

                    b.Property<string>("System")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Uploaded")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("UploadedById")
                        .HasColumnType("uuid");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UUID");

                    b.HasAlternateKey("Id", "Version");

                    b.HasIndex("AuthorId");

                    b.HasIndex("GameVersionId");

                    b.HasIndex("UploadedById");

                    b.ToTable("Mods");
                });

            modelBuilder.Entity("BeatMods2.Models.Mod_Tag_Join", b =>
                {
                    b.Property<Guid>("ModId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uuid");

                    b.HasKey("ModId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("Mod_Tag_Join");
                });

            modelBuilder.Entity("BeatMods2.Models.News", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("Edited")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Posted")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("System")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("BeatMods2.Models.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasAlternateKey("Name");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("BeatMods2.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("GithubId")
                        .HasColumnType("integer");

                    b.Property<string>("GithubToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Profile")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasAlternateKey("GithubToken");

                    b.HasAlternateKey("Name");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BeatMods2.Models.User_Group_Join", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("User_Group_Join");
                });

            modelBuilder.Entity("BeatMods2.Models.Download", b =>
                {
                    b.HasOne("BeatMods2.Models.Mod", null)
                        .WithMany("Downloads")
                        .HasForeignKey("ModUUID");
                });

            modelBuilder.Entity("BeatMods2.Models.GameVersion_Group_Join", b =>
                {
                    b.HasOne("BeatMods2.Models.GameVersion", "GameVersion")
                        .WithMany("VisibleTo")
                        .HasForeignKey("GameVersionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeatMods2.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BeatMods2.Models.Mod", b =>
                {
                    b.HasOne("BeatMods2.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeatMods2.Models.GameVersion", "GameVersion")
                        .WithMany()
                        .HasForeignKey("GameVersionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeatMods2.Models.User", "UploadedBy")
                        .WithMany()
                        .HasForeignKey("UploadedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("BeatMods2.Models.ModRange", "ConflictsWith", b1 =>
                        {
                            b1.Property<Guid>("ModUUID")
                                .HasColumnType("uuid");

                            b1.Property<string>("Id")
                                .HasColumnType("text");

                            b1.Property<string>("VersionRange")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("ModUUID", "Id");

                            b1.ToTable("Mods_ConflictsWith");

                            b1.WithOwner()
                                .HasForeignKey("ModUUID");
                        });

                    b.OwnsMany("BeatMods2.Models.ModRange", "DependsOn", b1 =>
                        {
                            b1.Property<Guid>("ModUUID")
                                .HasColumnType("uuid");

                            b1.Property<string>("Id")
                                .HasColumnType("text");

                            b1.Property<string>("VersionRange")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("ModUUID", "Id");

                            b1.ToTable("Mods_DependsOn");

                            b1.WithOwner()
                                .HasForeignKey("ModUUID");
                        });
                });

            modelBuilder.Entity("BeatMods2.Models.Mod_Tag_Join", b =>
                {
                    b.HasOne("BeatMods2.Models.Mod", "Mod")
                        .WithMany("Tags")
                        .HasForeignKey("ModId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeatMods2.Models.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BeatMods2.Models.News", b =>
                {
                    b.HasOne("BeatMods2.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BeatMods2.Models.User_Group_Join", b =>
                {
                    b.HasOne("BeatMods2.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeatMods2.Models.User", "User")
                        .WithMany("Groups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

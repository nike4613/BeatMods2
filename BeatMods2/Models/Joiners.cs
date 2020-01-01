using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeatMods2.Models
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    public class GameVersion_Group_Join
    {
        internal static void ConfigureModel(ModelBuilder model)
        {
            model.Entity<GameVersion_Group_Join>()
                .HasKey(j => new { j.GameVersionId, j.GroupId });
            model.Entity<GameVersion_Group_Join>()
                .HasOne(j => j.GameVersion)
                .WithMany(v => v.VisibleTo)
                .HasForeignKey(j => j.GameVersionId);
            model.Entity<GameVersion_Group_Join>()
                .HasOne(j => j.Group)
                .WithMany(v => v.ExplicitlyVisibleGameVersions)
                .HasForeignKey(j => j.GroupId);
        }
        public Guid GameVersionId { get; set; }
        public Guid GroupId { get; set; }

        [Required]
        public GameVersion GameVersion { get; set; }
        [Required]
        public Group Group { get; set; }
    }
    public class User_Group_Join
    {
        internal static void ConfigureModel(ModelBuilder model)
        {
            model.Entity<User_Group_Join>()
                .HasKey(j => new { j.UserId, j.GroupId });
            model.Entity<User_Group_Join>()
                .HasOne(j => j.User)
                .WithMany(v => v.Groups)
                .HasForeignKey(j => j.UserId);
            model.Entity<User_Group_Join>()
                .HasOne(j => j.Group)
                .WithMany(g => g.Users)
                .HasForeignKey(j => j.GroupId);
        }

        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }

        [Required]
        public User User { get; set; }
        [Required]
        public Group Group { get; set; }
    }

    public class Mod_Tag_Join
    {
        internal static void ConfigureModel(ModelBuilder model)
        {
            model.Entity<Mod_Tag_Join>()
                .HasKey(j => new { j.ModId, j.TagId });
            model.Entity<Mod_Tag_Join>()
                .HasOne(j => j.Mod)
                .WithMany(v => v.Tags)
                .HasForeignKey(j => j.ModId);
            model.Entity<Mod_Tag_Join>()
                .HasOne(j => j.Tag)
                .WithMany(t => t.Mods)
                .HasForeignKey(j => j.TagId);
        }

        public Guid ModId { get; set; }
        public Guid TagId { get; set; }

        [Required]
        public Mod Mod { get; set; }
        [Required]
        public Tag Tag { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
}

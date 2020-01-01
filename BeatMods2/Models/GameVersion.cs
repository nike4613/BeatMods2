using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Version = SemVer.Version;

namespace BeatMods2.Models
{
    public enum Visibility
    {
        Public, GroupsOnly
    }

    public class GameVersion
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        internal static void ConfigureModel(ModelBuilder model)
        {
            model.Entity<GameVersion>()
                .HasAlternateKey(d => d.VersionName); // should be unique on target mod and type

            model.Entity<GameVersion>()
                .Property(d => d.Visibility)
                .HasConversion(
                    v => v.ToString(),
                    v => (Visibility)Enum.Parse(typeof(Visibility), v));

            GameVersion_Group_Join.ConfigureModel(model);
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string VersionName { get; set; }
        [Required]
        public string SteamBuildId { get; set; }

        public Visibility Visibility { get; set; }

        public List<GameVersion_Group_Join> VisibleTo { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}
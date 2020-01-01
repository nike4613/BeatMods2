using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Version = SemVer.Version;

namespace BeatMods2.Models
{
    public enum Approval
    {
        Approved, Declined, Pending, Inactive
    }

    public enum System
    {
        PC, Quest
    }

    public class Mod
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        internal static void ConfigureModel(ModelBuilder model)
        {
            model.Entity<Mod>()
                .HasAlternateKey(d => new { d.Id, d.Version }); // should be unique on target mod and type
            model.Entity<Mod>()
                .Property(d => d.ApprovalState)
                .HasConversion(
                    v => v.ToString(),
                    v => (Approval)Enum.Parse(typeof(Approval), v));
            model.Entity<Mod>()
                .Property(d => d.System)
                .HasConversion(
                    v => v.ToString(),
                    v => (System)Enum.Parse(typeof(System), v));
            model.Entity<Mod>()
                .Property(d => d.Version)
                .HasConversion(
                    v => v.ToString(),
                    v => new Version(v, false));
            model.Entity<Mod>()
                .OwnsMany(m => m.DependsOn, ModRange.Configure);
            model.Entity<Mod>()
                .OwnsMany(m => m.ConflictsWith, ModRange.Configure);

            Mod_Tag_Join.ConfigureModel(model);
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UUID { get; set; }
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public User? Author { get; set; }
        [Required]
        public User? UploadedBy { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string License { get; set; }
        [Required]
        public Version Version { get; set; }
        public DateTime Uploaded { get; set; }
        public DateTime? Approved { get; set; }
        public Approval ApprovalState { get; set; }
        [Required]
        public GameVersion? GameVersion { get; set; }
        public System System { get; set; }
        public bool Required { get; set; }

        public List<ModRange> DependsOn { get; set; } = new List<ModRange>();
        public List<ModRange> ConflictsWith { get; set; } = new List<ModRange>();

        public List<Download> Downloads { get; set; } = new List<Download>();
        
        public List<Mod_Tag_Join> Tags { get; set; } = new List<Mod_Tag_Join>();
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}

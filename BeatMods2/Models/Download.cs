using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeatMods2.Models
{
    public enum DownloadType
    {
        Steam, Oculus, UniversalPC, Quest
    }

    public class Download
    {
        internal static void ConfigureModel(ModelBuilder model)
        {
            model.Entity<Download>()
                .HasKey(d => new { d.Mod, d.Type }); // should be unique on target mod and type
            model.Entity<Download>()
                .Property(d => d.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (DownloadType)Enum.Parse(typeof(DownloadType), v));
            model.Entity<Download>()
                .Property(d => d.CdnFile)
                .HasConversion(
                    v => v.ToString(),
                    v => new Uri(v));
            model.Entity<Download>()
                .Property(d => d.Hashes)
                .HasConversion(
                    v => v.ToString(),
                    v => JObject.Parse(v));
        }

        [ForeignKey(nameof(Mod)), Required]
        public Guid Mod { get; set; }

        public DownloadType Type { get; set; }
        [Required]
        public Uri CdnFile { get; set; }
        [Required]
        public JObject Hashes { get; set; }
    }
}
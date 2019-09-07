using Microsoft.EntityFrameworkCore;
using SemVer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeatMods2.Models
{
    [Owned]
    public class ModRange
    {
        internal static void ConfigureModel(ModelBuilder model)
        {
            model.Entity<ModRange>()
                .Property(d => d.VersionRange)
                .HasConversion(
                    v => v.ToString(),
                    v => new Range(v, false));
        }

        [Required]
        public string Id { get; set; }
        [Required]
        public Range VersionRange { get; set; }
    }
}

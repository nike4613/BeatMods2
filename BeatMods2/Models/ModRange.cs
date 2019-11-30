using Microsoft.EntityFrameworkCore;
using SemVer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeatMods2.Models
{
    [Owned]
    public class ModRange
    {
        internal static void Configure<N>(OwnedNavigationBuilder<N, ModRange> b) where N : class
        {
            b.Property(d => d.VersionRange)
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

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
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
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
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeatMods2.Models
{
    public static class AuthCodeTempStoreExtensions
    {
        public static void ClearExpired(this DbSet<AuthCodeTempStore> set)
            => AuthCodeTempStore.ClearExpired(set);
    }
    public class AuthCodeTempStore
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        internal static void ConfigureModel(ModelBuilder model)
        {
            model.Entity<AuthCodeTempStore>()
                .HasAlternateKey(a => a.GitHubBearer);
        }

        public static void ClearExpired(DbSet<AuthCodeTempStore> set)
            => set.RemoveRange(set.Where(a => a.Expires < DateTime.Now));
        
        // default to now plus 10 minutes
        [Required]
        public DateTime Expires { get; set; } = DateTime.Now + TimeSpan.FromMinutes(10);

        [Key]
        public string Code { get; set; }

        public string GitHubBearer { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeatMods2.Models
{
    public class ModRepoContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public ModRepoContext(DbContextOptions<ModRepoContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Tag.ConfigureModel(modelBuilder);
            User.ConfigureModel(modelBuilder);
            Models.News.ConfigureModel(modelBuilder);
            Mod.ConfigureModel(modelBuilder);
            Group.ConfigureModel(modelBuilder);
            GameVersion.ConfigureModel(modelBuilder);
            Download.ConfigureModel(modelBuilder);
        }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Mod> Mods { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GameVersion> GameVersions { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}

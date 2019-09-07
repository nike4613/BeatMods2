using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeatMods2.Models
{
    public class User
    {
        internal static void ConfigureModel(ModelBuilder model)
        {
            model.Entity<User>()
                .HasAlternateKey(d => d.Name); // should be unique on target mod and type
            model.Entity<User>()
                .HasAlternateKey(d => d.GithubId); // should be unique on target mod and type

            User_Group_Join.ConfigureModel(model);
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime Created { get; set; }
        [Required]
        public string Profile { get; set; }
        [Required]
        public string GithubId { get; set; }

        public ICollection<User_Group_Join> Groups { get; set; }
    }
}
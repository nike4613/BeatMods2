using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeatMods2.Models
{
    public class User
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        internal static void ConfigureModel(ModelBuilder model)
        {
            model.Entity<User>()
                .HasAlternateKey(d => d.Name);
            model.Entity<User>()
                .HasAlternateKey(d => d.GithubId);

            User_Group_Join.ConfigureModel(model);
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public string Profile { get; set; } = "";
        [Required]
        public string GithubToken { get; set; }
        [Required]
        public int GithubId { get; set; }

        public ICollection<User_Group_Join> Groups { get; set; } = new List<User_Group_Join>();
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public void AddGroup(Group group)
        {
            if (Groups.Any(j => j.GroupId == group.Id)) return;
            var joiner = new User_Group_Join
            {
                UserId = Id,
                GroupId = group.Id,
                User = this,
                Group = group
            };
            Groups.Add(joiner);
        }

        public bool HasPermission(Permission perm)
            => Groups.Any(j => j.Group.HasPermission(perm));
    }
}
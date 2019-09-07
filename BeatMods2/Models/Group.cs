using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BeatMods2.Models
{
    public enum Permission
    {
        gameversion_add, gameversion_edit, mod_create, mod_edit, mod_reposess, user_delete,
        group_add, group_edit, group_delete, mod_see_pending, mod_approve_deny, user_edit_groups,
        news_edit, news_add, mod_upload_as
    }

    public class Group
    {
        internal static void ConfigureModel(ModelBuilder model)
        {
            model.Entity<Group>()
                .HasAlternateKey(d => d.Name); // should be unique on target mod and type
            model.Entity<Group>()
                .Property(g => g.Permissions)
                .HasConversion(
                    v => v.Select(p => p.ToString()).ToList(),
                    v => v.Select(p => (Permission)Enum.Parse(typeof(Permission), p)).ToList());
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
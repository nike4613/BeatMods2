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
        GameVersion_Add, GameVersion_Edit, Mod_Create, Mod_Edit, Mod_Reposess, User_Delete,
        Group_Add, Group_Edit, Group_Delete, Mods_ViewPending, Mods_ApproveDeny, User_EditGroups,
        News_Edit, News_Add, Mod_UploadAs
    }

    public class Group
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
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

#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}
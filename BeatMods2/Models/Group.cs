using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BeatMods2.Models
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class PermissionAttribute : Attribute
    {
        public string Category { get; }
        public string Name { get; }
        public PermissionAttribute(string category, string name)
        {
            Category = category;
            Name = name;
        }
    }
    public enum Permission
    {
        GameVersion_Add, GameVersion_Edit, Mod_Create, Mod_Edit, Mod_Reposess, User_Delete,
        Group_Add, Group_Edit, Group_Delete, Mods_ViewPending, User_EditGroups,
        News_Edit, News_Add, Mod_UploadAs, 
        [Permission("", nameof(Administrate))]
        Administrate,
        [Permission("Mods", "Approve/Deny")]
        Mods_ApproveDeny
    }

    public static class PermissionExtensions
    {
        private static readonly Dictionary<Permission, PermissionAttribute?> dispInfos
            = new Dictionary<Permission, PermissionAttribute?>();
        public static PermissionAttribute? GetDisplayInfo(this Permission perm)
        {
            if (dispInfos.TryGetValue(perm, out var info)) return info;
            info = GetDisplayInfoInternal(perm);
            dispInfos.Add(perm, info);
            return info;
        }
        private static PermissionAttribute? GetDisplayInfoInternal(Permission perm)
        {
            var member = typeof(Permission).GetMember(perm.ToString()).FirstOrDefault();
            if (member == null) return null;

            var attr = member.GetCustomAttribute<PermissionAttribute>();
            if (attr != null) return attr;

            var name = perm.ToString();
            var parts = name.Split('_');
            Debug.Assert(parts.Length == 2);

            static string DeCamelCase(string n)
            {
                var sb = new StringBuilder(n.Length*2);
                foreach (var chr in n)
                {
                    if (sb.Length > 0 && char.IsUpper(chr))
                        sb.Append(' ');
                    sb.Append(chr);
                }
                return sb.ToString();
            }

            parts = parts.Select(DeCamelCase).ToArray();
            return new PermissionAttribute(parts[0], parts[1]);
        }
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

        public const string DefaultGroupName = "default";
        [Required]
        public string Name { get; set; }
        public List<Permission> Permissions { get; set; }

#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}
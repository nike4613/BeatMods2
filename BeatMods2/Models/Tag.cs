using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeatMods2.Models
{
    public class Tag
    {
        internal static void ConfigureModel(ModelBuilder model)
        {
            model.Entity<Tag>()
                .HasAlternateKey(d => d.Name); // should be unique on target mod and type
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}

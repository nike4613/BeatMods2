using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BeatMods2.Models
{
    public class News
    {
        internal static void ConfigureModel(ModelBuilder model)
        {
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public User Author { get; set; }
        [Required]
        public string Body { get; set; }
        public DateTime Posted { get; set; }
        public DateTime? Edited { get; set; }
        public System? System { get; set; }
    }
}

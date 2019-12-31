using BeatMods2.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeatMods2.Configuration
{
    public class CoreAuth
    {
        public string JwtSecret { get; set; } = "";
        public TimeSpan JwtExpiry { get; set; }
    }
}
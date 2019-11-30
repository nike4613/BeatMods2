using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatMods2.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BeatMods2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mode = Configuration.GetValue<string>("DBMode");
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            Action<DbContextOptionsBuilder> builder = null;

            if (mode == "postgres")
                builder = options => options.UseNpgsql(connectionString);
            else
                throw new Exception($"Invalid mode {mode}");

            services.AddDbContext<ModRepoContext>(builder);

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
#if DEBUG
            app.UseDeveloperExceptionPage();
#endif

            app.UseHttpsRedirection();

            app.UseMvc();
        }
    }
}

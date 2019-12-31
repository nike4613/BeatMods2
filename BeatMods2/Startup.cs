using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BeatMods2.Configuration;
using BeatMods2.Models;
using BeatMods2.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Octokit;

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

            var ghAuthSettings = new GitHubAuth();
            Configuration.Bind("GithubAuth", ghAuthSettings);
            services.AddSingleton(ghAuthSettings);

            var coreAuthSettings = new CoreAuth();
            Configuration.Bind("CoreAuth", coreAuthSettings);
            services.AddSingleton(coreAuthSettings);

            var asmVersion = new SemVer.Version(Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion);
            services.AddSingleton(asmVersion);

            services.AddScoped<GitHubClient>(s =>
                new GitHubClient(
                    new Connection(
                        new ProductHeaderValue("BeatMods2", 
                            s.GetRequiredService<SemVer.Version>().ToString()))));

            services
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped(x => x
                    .GetRequiredService<IUrlHelperFactory>()
                    .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));
            services.AddSingleton<SymmetricAlgorithm>(s => {
                using var key = Utils.CoerceToSize(256 / 8, s.GetRequiredService<GitHubAuth>().StateEncKeyBytes);
                return new AesManaged()
                {
                    Key = key.Memory.ToArray(),
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.ISO10126
                };
            });

            Action<DbContextOptionsBuilder> builder;

            if (mode == "postgres")
                builder = options => options.UseNpgsql(connectionString);
            else
                throw new Exception($"Invalid mode {mode}");

            services.AddDbContext<ModRepoContext>(builder);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
                    Configuration.Bind("JwtSettings", options);
                    options.TokenValidationParameters.IssuerSigningKey 
                        = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(coreAuthSettings.JwtSecret));
                });

            services.AddRouting();

            services.AddControllers()
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
#if DEBUG
            app.UseDeveloperExceptionPage();
#endif

            app.UseHttpsRedirection();

            app.UseCors(p =>
                p.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(rb =>
            {
                rb.MapControllers();
            });
        }
    }
}

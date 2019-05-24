using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedAutorizationOptions;

namespace AuthorizationService
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<BaseContext>(options =>

            options.UseSqlServer(Configuration.GetConnectionString("UsersDatabase")));




            services.AddIdentity<IdentityUser, IdentityRole>(cfg =>
            {
                cfg.Password.RequireDigit = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<BaseContext>();

            //  services.AddIdentityCore<AppAccount>().AddEntityFrameworkStores<BaseContext>();



            var accountOptions = new AuthorizationOptions()

            {
                ISSUER = "authServer",
                AUDIENCE = "GateWay"
            };
            var tokenGenerator = new TokenGenerator(accountOptions);

            services.AddSingleton<ITokenGenerator>(tokenGenerator);

            services.AddSingleton<AuthorizationOptions>(accountOptions);

            //      configuration.GetSection("Security:Tokens").Bind(accountOptions);

            //   services.Configure<AuthOptions>(options => configuration.GetSection("Security:Tokens").Bind(options));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = accountOptions.GetParameters();

                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            app.UseAuthentication();
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

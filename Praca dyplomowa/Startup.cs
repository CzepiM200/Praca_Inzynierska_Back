using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Google;
using Praca_dyplomowa.Helpers;
using Praca_dyplomowa.Services;
using Praca_dyplomowa.Context;

namespace Praca_dyplomowa
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            // ??? - !
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // configure strongly typed settings objects
            var appSettingsSection = _configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();

            // configure Regions for application services
            services.AddScoped<IRegionService, RegionService>();

            // configure Trainings for application services
            services.AddScoped<IActivityService, ActivityService>();

            // connection string do bazy
            services.AddDbContext<ProgramContext>(options =>
                options.UseMySql("server=localhost;port=3306;database=organizer;uid=root;password=admin"));


            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            //var key = Encoding.ASCII.GetBytes("Has³o123");
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            });
            //.AddGoogle(options =>
            //{
            //    options.ClientId = "303201248383-b4tn3cojmbf9b1hovndfl13vm8uleans.apps.googleusercontent.com";
            //    options.ClientSecret = "9VpO2AMLbI0m72wQU5aNeLxJ";
            //});
            //.AddJwtBearer(x =>
            //{
            //    x.Events = new JwtBearerEvents
            //    {
            //        OnTokenValidated = context =>
            //        {
            //            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
            //            var userId = int.Parse(context.Principal.Identity.Name);
            //            var user = userService.GetById(userId);
            //            if (user == null)
            //            {
            //                // return unauthorized if user no longer exists
            //                context.Fail("Unauthorized");
            //            }
            //            return Task.CompletedTask;
            //        }
            //    };
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ProgramContext programContext)
        {
            programContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // HTTP Strict Transport Security Protocol (HSTS) Middleware (UseHsts) adds the Strict-Transport-Security header.
                app.UseHsts();
            }

            // HTTPS Redirection Middleware (UseHttpsRedirection) redirects HTTP requests to HTTPS.
            app.UseHttpsRedirection();

            // Cookie Policy Middleware (UseCookiePolicy) conforms the app to the EU General Data Protection Regulation (GDPR) regulations.

            // Routing Middleware (UseRouting) to route requests.
            app.UseRouting();


            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // Sprawdzenie kim jest u¿ytkownik
            // Authentication Middleware (UseAuthentication) attempts to authenticate the user before they're allowed access to secure resources.
            app.UseAuthentication();

            // Sprawdzenie uprawnieñ u¿ytkownika
            // Authorization Middleware (UseAuthorization) authorizes a user to access secure resources.
            app.UseAuthorization();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            

            // Endpoint Routing Middleware (UseEndpoints with MapRazorPages) to add Razor Pages endpoints to the request pipeline.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
        }
    }
}

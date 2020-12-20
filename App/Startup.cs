using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using App.Repositories;
using App.Repositories.DbContexts;
using App.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        public void ConfigureServices(IServiceCollection services)
        {
            AddSeedData(services);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<AppDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DB"));
            });



            /* add identity - authentication */
            services.AddIdentityCore<UserAccount>(opt => {
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager<SignInManager<UserAccount>>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.ApplicationScheme, config =>
                {
                    config.Cookie.Name = "LoginCookie";
                    config.LoginPath = "/Account/Login";
                    config.LogoutPath = "/Account/Logout";
                    config.AccessDeniedPath = "/Account/AccessDenied";
                });
            services.AddAuthorization();


            services.AddMvc(opts => opts.EnableEndpointRouting = false);
            services.AddTransient<IGameAccountRepo, GameAccountRepo>();
            services.AddTransient<IUserAccountRepo, UserAccountRepo>();
            services.AddTransient<IRankRepo, RankRepo>();
        }

        private void AddSeedData(IServiceCollection services)
        {
            var superAdminAccount = Configuration.GetSection("SuperAdminAccount").Get<SuperAdminSeedAccount>();
            services.AddSingleton(superAdminAccount);

            var adminAccount = Configuration.GetSection("AdminAccount").Get<AdminSeedAccount>();
            services.AddSingleton(adminAccount);

            var smtpAccount = Configuration.GetSection("SmtpAccount").Get<MailkitMetaData>();
            services.AddSingleton(smtpAccount);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*if (env.IsDevelopment())
            {
            }*/


            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseMvc(routes => {
                routes.MapRoute(name: null, template: "Rank{rankId:int}/Page{pageIndex:int}", defaults: new { controller = "Home", action = "Index" });
                routes.MapRoute(name: null, template: "Rank{rankId:int}", defaults: new { controller = "Home", action = "Index" });
                routes.MapRoute(name: null, template: "Page{pageIndex:int}", defaults: new { controller = "Home", action = "Index", pageIndex = 1 });

                routes.MapRoute(name: null, template: "Price_From{from:int}_To{to:int}", defaults: new { controller = "Home", action = "Price" });
                routes.MapRoute(name: null, template: "Price_From{from:int}", defaults: new { controller = "Home", action = "Price", to = 9999999999 });
                routes.MapRoute(name: null, template: "Price_To{to:int}", defaults: new { controller = "Home", action = "Price", from = 0 });


                routes.MapRoute(name: null, template: "Home/Buy/acc_{accId:int}", defaults: new { controller = "Home", action = "Buy" });
                routes.MapRoute(name: null, template: "acc_{accId:int}", defaults: new { controller = "Home", action = "GameAccountDetail" });


                routes.MapRoute(name: null, template: "Dashboard", defaults: new { controller = "Admin", action = "Dashboard" });

                routes.MapRoute(name: null, template: "", defaults: new { controller = "Home", action = "Index" });
                routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
            });


            AppDbContext.SeedData(app.ApplicationServices).Wait();
        }

    }
}

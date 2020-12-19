using App.Infrastructures;
using App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Repositories.DbContexts
{
    public class AppDbContext : IdentityDbContext<UserAccount>
    {
        public DbSet<GameAccount> GameAccounts { get; set; }
        public DbSet<Rank> Ranks { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }


        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ignore the Id field from updating...
            modelBuilder.Entity<GameAccount>(builder =>
            {
                builder
                    .Property(e => e.Id).ValueGeneratedOnAdd()
                        .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            });
        }*/



        public static async Task SeedData(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRolesAsync(roleManager);


            var userManager = services.GetRequiredService<UserManager<UserAccount>>();
            await EnsureAdminAccountsAsync(userManager, services);


            SeedRanks(services);
            /*SeedGameAccounts(services);*/
        }

        private static async Task EnsureAdminAccountsAsync(UserManager<UserAccount> userManager, IServiceProvider services)
        {
            #region Seed Super Admin
            var superAdminModel = services.GetRequiredService<SuperAdminSeedAccount>();
            var superAdmin = await userManager.FindByEmailAsync(superAdminModel.Email);

            if (superAdmin == null)
            {
                var superAdminUser = superAdminModel.ToUserAccount();
                var userCreateResult = await userManager.CreateAsync(superAdminUser, superAdminModel.Password);


                if (userCreateResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdminUser, Constants.SuperAdminRole);
                }
            }
            #endregion


            #region Seed Admin
            var adminModel = services.GetRequiredService<AdminSeedAccount>();
            var user = await userManager.FindByEmailAsync(adminModel.Email);

            if (user == null)
            {
                var adminUser = adminModel.ToUserAccount();
                var userCreateResult = await userManager.CreateAsync(adminUser, adminModel.Password);


                if (userCreateResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, Constants.AdminRole);
                }
            }
            #endregion

        }

        private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            try
            {
                var adminRoleExisted = await roleManager.RoleExistsAsync(Constants.AdminRole);
                var clientRoleExisted = await roleManager.RoleExistsAsync(Constants.ClientRole);
                var superAdminRoleExisted = await roleManager.RoleExistsAsync(Constants.SuperAdminRole);


                if (!superAdminRoleExisted)
                {
                    await roleManager.CreateAsync(new IdentityRole(Constants.SuperAdminRole));
                }


                if (!adminRoleExisted)
                {
                    await roleManager.CreateAsync(new IdentityRole(Constants.AdminRole));
                }


                if (!clientRoleExisted)
                {
                    await roleManager.CreateAsync(new IdentityRole(Constants.ClientRole));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error roles seeding...\n" + ex.Message);
            }
        }



        private static void SeedRanks(IServiceProvider services)
        {
            var context = services.GetRequiredService<AppDbContext>();
            

            if (!context.Ranks.Any())
            {
                context.Ranks.AddRange(
                    new Rank { Name = "Chưa Rank" },
                    new Rank { Name = "Đồng" },
                    new Rank { Name = "Bạc" },
                    new Rank { Name = "Vàng" },
                    new Rank { Name = "Bạch Kim" },
                    new Rank { Name = "Kim Cương" },
                    new Rank { Name = "Tinh Anh" },
                    new Rank { Name = "Cao Thủ" }
                );

                context.SaveChanges();
            }
        }

        private static void SeedGameAccounts(IServiceProvider services)
        {
            using (var context = services.GetRequiredService<AppDbContext>())
            {
                if (!context.GameAccounts.Any())
                {
                    context.GameAccounts.AddRange(
                        new GameAccount {
                            ImageUrls = "a",
                            Price = 1000000,
                            LoginName = "muasaobang",
                            Password = "abc",
                            RankName = "Kim Cương",
                            GoldsCount = 10000,
                            SkinsCount = 120,
                            HeroesCount = 75
                        },
                        new GameAccount
                        {
                            Price = 2400000,
                            LoginName = "muasaobang1",
                            Password = "abc",
                            ImageUrls = "a",
                            RankName = "Bạch Kim",
                            GoldsCount = 10000,
                            SkinsCount = 120,
                            HeroesCount = 75
                        }, new GameAccount
                        {
                            Price = 700000,
                            LoginName = "muasaobang2",
                            Password = "abc",
                            ImageUrls = "a",
                            RankName = "Tinh Anh",
                            GoldsCount = 10000,
                            SkinsCount = 120,
                            HeroesCount = 75
                        }, new GameAccount
                        {
                            Price = 300000,
                            LoginName = "muasaobang3",
                            Password = "abc",
                            ImageUrls = "a",
                            RankName = "Vàng",
                            GoldsCount = 10000,
                            SkinsCount = 120,
                            HeroesCount = 75
                        }, new GameAccount
                        {
                            Price = 120000,
                            LoginName = "muasaobang4",
                            Password = "abc",
                            ImageUrls = "a",
                            RankName = "Kim Cương",
                            GoldsCount = 10000,
                            SkinsCount = 120,
                            HeroesCount = 75
                        }
                    );
                }

                context.SaveChanges();
            }
        }


    }
}

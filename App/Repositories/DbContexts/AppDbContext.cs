using App.Infrastructures;
using App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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



        public static async Task SeedData(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            if (await EnsureRolesAsync(roleManager))
            {
                var userManager = services.GetRequiredService<UserManager<UserAccount>>();
                var adminAccount = services.GetRequiredService<AdminSeedAccount>();

                var createAccountResult = await EnsureAdminAccountAsync(userManager, adminAccount);
            }


            SeedRanks(services);
            /*SeedGameAccounts(services);*/
        }

        private static async Task<bool> EnsureAdminAccountAsync(UserManager<UserAccount> userManager, AdminSeedAccount adminAccount)
        {
            var user = await userManager.FindByEmailAsync(adminAccount.Email);
            if (user != null) return true;


            var adminUser = adminAccount.ToUserAccount();
            var userCreateResult = await userManager.CreateAsync(adminUser, adminAccount.Password);

            if (userCreateResult.Succeeded)
            {
                var addRoleResult = await userManager.AddToRoleAsync(adminUser, Constants.AdminRole);
                return addRoleResult.Succeeded;
            }

            return false;
        }

        private static async Task<bool> EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var adminRoleExisted = await roleManager.RoleExistsAsync(Constants.AdminRole);
            var clientRoleExisted = await roleManager.RoleExistsAsync(Constants.ClientRole);
            if (adminRoleExisted && clientRoleExisted) return true;


            var adminRoleCreateResult = await roleManager.CreateAsync(new IdentityRole(Constants.AdminRole));
            var clientRoleCreateResult = await roleManager.CreateAsync(new IdentityRole(Constants.ClientRole));

            return adminRoleCreateResult.Succeeded && clientRoleCreateResult.Succeeded;
        }



        private static void SeedRanks(IServiceProvider services)
        {
            var context = services.GetRequiredService<AppDbContext>();
            

            if (!context.Ranks.Any())
            {
                context.Ranks.AddRange(
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

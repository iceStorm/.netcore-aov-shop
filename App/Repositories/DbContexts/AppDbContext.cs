using App.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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


    }
}

using App.Infrastructures;
using App.Models;
using App.Repositories.DbContexts;
using App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Repositories
{
    public class UserAccountRepo : IUserAccountRepo
    {
        private AppDbContext dbContext;

        public UserAccountRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }



        public IQueryable<UserAccount> Accounts(string roleType)
        {
            var roleId = dbContext.Roles.Where(r => r.Name == roleType).Select(r => r.Id).FirstOrDefault();
            var usersId = dbContext.UserRoles.Where(ur => ur.RoleId == roleId).Select(ur => ur.UserId);

            return dbContext.Users.Where(u => usersId.Contains(u.Id)).Include(acc => acc.BoughtAccounts);
        }


        public bool DeleteAccount(UserAccount account)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Id == account.Id);


            if (user != null)
            {
                var removedUser = dbContext.Users.Remove(user);
                return dbContext.SaveChanges() == 1;
            }


            return false;
        }

        public void SaveAccount(UserAccount account)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Id == account.Id);


            if (user != null)
            {
                account.PasteValues(user);
                dbContext.SaveChanges();
            }
        }


    }
}

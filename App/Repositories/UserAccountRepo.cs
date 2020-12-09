using App.Infrastructures;
using App.Models;
using App.Repositories.DbContexts;
using App.Repositories.Interfaces;
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
            var adminRoleId = dbContext.Roles.Where(r => r.Name == roleType).Select(r => r.Id).FirstOrDefault();
            var adminUsersId = dbContext.UserRoles.Where(ur => ur.RoleId == adminRoleId).Select(ur => ur.UserId);

            return dbContext.Users.Where(u => adminUsersId.Contains(u.Id));
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

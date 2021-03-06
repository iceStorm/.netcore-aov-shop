﻿using App.Models;
using App.Repositories.DbContexts;
using App.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Repositories
{
    public class GameAccountRepo : IGameAccountRepo
    {
        private AppDbContext dbContext;

        public GameAccountRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }



        public IQueryable<GameAccount> Accounts => dbContext.GameAccounts.Include(acc => acc.Rank);

        public bool DeleteGameAccount(string loginName)
        {
            var gameAcc = dbContext.GameAccounts.Where(acc => acc.LoginName == loginName).FirstOrDefault();
            if (gameAcc != null)
            {
                dbContext.GameAccounts.Remove(gameAcc);
                return dbContext.SaveChanges() == 1;
            }

            return false;
        }

        public void SaveGameAccount(GameAccount account)
        {
            var accEntry = dbContext.GameAccounts.FirstOrDefault(acc => acc.Id == account.Id);
            if (accEntry != null)
            {
                accEntry.CopyValues(account);
                dbContext.GameAccounts.Update(accEntry);
                dbContext.SaveChanges();
            }
            else
            {
                dbContext.GameAccounts.Add(account);
                dbContext.SaveChanges();
            }
        }

    }
}

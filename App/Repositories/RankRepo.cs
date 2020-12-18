using App.Models;
using App.Repositories.DbContexts;
using App.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Repositories
{
    public class RankRepo : IRankRepo
    {
        private AppDbContext dbContext;

        public RankRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }



        public IQueryable<Rank> Ranks => dbContext.Ranks;

        public bool DeleteRank(string rankName)
        {
            var rank = dbContext.Ranks.FirstOrDefault(r => r.Name == rankName);
            if (rank != null)
            {
                dbContext.Ranks.Remove(rank);
                return dbContext.SaveChanges() == 1;
            }

            return false;
        }

        public void SaveRank(Rank rank)
        {
            var rankEntry = dbContext.Ranks.FirstOrDefault(r => r.Name == rank.Name);
            if (rankEntry != null)
            {
                rankEntry.CopyValues(rank);
                dbContext.Ranks.Update(rankEntry);
                dbContext.SaveChanges();
            }
            else
            {
                dbContext.Ranks.Add(rank);
                dbContext.SaveChanges();
            }
        }
    }
}

using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Repositories.Interfaces
{
    public interface IRankRepo
    {
        public IQueryable<Rank> Ranks { get; }
        void SaveRank(Rank rank);
        bool DeleteRank(string rankName);
    }
}

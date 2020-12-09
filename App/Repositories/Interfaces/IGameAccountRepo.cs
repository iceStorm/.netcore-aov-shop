using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Repositories.Interfaces
{
    public interface IGameAccountRepo
    {
        public IQueryable<GameAccount> Accounts { get; }
        void SaveGameAccount(GameAccount account);
        bool DeleteGameAccount(string loginName);
    }
}

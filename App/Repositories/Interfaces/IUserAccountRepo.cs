using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Repositories.Interfaces
{
    public interface IUserAccountRepo
    {
        IQueryable<UserAccount> Accounts(string roleType);
        UserAccount SaveAccount(UserAccount account);
        bool DeleteAccount(UserAccount account);
    }
}

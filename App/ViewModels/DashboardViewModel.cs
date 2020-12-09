using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public class DashboardViewModel
    {
        public IQueryable<GameAccount> GameAccounts { get; set; }
    }
}

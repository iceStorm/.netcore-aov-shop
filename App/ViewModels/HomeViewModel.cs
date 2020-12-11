using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public class HomeViewModel
    {
        public IQueryable<GameAccount> AccountsList { get; set; }

        public PagingInfo PagingInfo { get; set; }

    }
}

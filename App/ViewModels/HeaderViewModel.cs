using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public class HeaderViewModel
    {

        public IQueryable<Rank> Ranks { get; set; }

        public UserAccount User { get; set; }

    }
}

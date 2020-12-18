using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public class AddGameAccountViewModel : GameAccount
    {
        public IEnumerable<Rank> RanksList { get; set; }
    }
}

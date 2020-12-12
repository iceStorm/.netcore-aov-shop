using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public class BuyViewModel
    {
        public UserAccount Buyer { get; set; }
        public GameAccount GameAccount { get; set; }
    }
}

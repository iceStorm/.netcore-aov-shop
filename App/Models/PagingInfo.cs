using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class PagingInfo
    {

        public int TotalItems { get; set; }

        public int ItemsPerPage { get; set; }

        public int CurrentPageIndex { get; set; }

        public int CurrentRankId { get; set; }

        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);

    }
}

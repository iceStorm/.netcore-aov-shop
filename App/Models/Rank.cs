using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    [Table ("Ranks")]
    public class Rank
    {
        [Key]
        [Required (ErrorMessage = "Vui lòng nhập tên Rank")]
        public string Name { get; set; }

        public void CopyValues(Rank source)
        {
            this.Name = source.Name;
        }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class GameAccount
    {
        [Key]
        [Required(ErrorMessage = "Vui lòng nhập Tên đăng nhập cho tài khoản")]
        public string LoginName { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu cho tài khoản")]
        public string Password { get; set; }



        [Required(ErrorMessage = "Vui lòng nhập đơn giá cho tài khoản")]
        public int Price { get; set; }

        

        [Required (ErrorMessage = "Vui lòng chọn ít nhất 1 ảnh")]
        public string ImageUrls { get; set; }



        #region Currency Units
        [Required(ErrorMessage = "Vui lòng nhập Số lượng Vàng")]
        public int GoldsCount { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập Số lượng Tướng")]
        public int HeroesCount { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập Số lượng Trang phục")]
        public int SkinsCount { get; set; }
        #endregion



        [Required(ErrorMessage = "Vui lòng chọn Rank")]
        [ForeignKey (nameof(Rank))]
        public string RankName { get; set; }

        [BindNever]
        public Rank Rank { get; set; }



        [ForeignKey (nameof(Buyer))]
        public string UserAccountId;
        public UserAccount Buyer { get; set; }




        public void CopyValues(GameAccount source)
        {
            Price = source.Price;
            Password = source.Password;
            ImageUrls = source.ImageUrls;
            HeroesCount = source.HeroesCount;
            SkinsCount = source.SkinsCount;
            RankName = source.RankName;
            Rank = source.Rank;
        }

    }
}

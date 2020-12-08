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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public bool Sold { get; set; } = false; //  Đã bán


        [NotMapped]
        [Required (ErrorMessage = "Vui lòng chọn ít nhất 1 ảnh")]
        public ICollection<string> ImageUrlsList { get; set; }
        [Required (ErrorMessage = "Vui lòng chọn ít nhất 1 ảnh")]
        public string ImageUrls
        {
            get { return string.Join(",", ImageUrlsList); }
            set { ImageUrlsList = value.Split(',').ToList(); }
        }



        #region Currency Units
        public int GemsCount { get; set; } = 0; //  Quân huy

        public int GemStonesCount { get; set; } = 0; //  Đá quý


        [Required(ErrorMessage = "Vui lòng nhập Số lượng Vàng")]
        public int GoldsCount { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập Số lượng Ruby")]
        public int RubiesCount { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập Số lượng Tướng")]
        public int HeroesCount { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập Số lượng Trang phục")]
        public int SkinsCount { get; set; }
        #endregion



        [Required(ErrorMessage = "Vui lòng chọn Rank")]
        [ForeignKey (nameof(Rank))]
        public string RankName;

        [BindNever]
        public Rank Rank { get; set; }

        public int RankStartsCount { get; set; }



        public void Copy(GameAccount source)
        {
            Id = source.Id;
            Sold = source.Sold;
            ImageUrlsList = source.ImageUrlsList;
            ImageUrls = source.ImageUrls;
            GemsCount = source.GemsCount;
            GemStonesCount = source.GemStonesCount;
            GoldsCount = source.GoldsCount;
            RubiesCount = source.RubiesCount;
            HeroesCount = source.HeroesCount;
            SkinsCount = source.SkinsCount;
            RankName = source.RankName;
            Rank = source.Rank;
            RankStartsCount = source.RankStartsCount;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OnlineExamSytem.Models
{
    public class Question
    {
        [Key]
        public int ID { get; set; }
        [Display(Name="Soru")]
        [Required]
        public string questionLabel { get; set; }
        [Display(Name = "Seçenek 1")]
        [Required]
        public string choiceA { get; set; }
        [Display(Name = "Seçenek 2")]
        [Required]
        public string choiceB { get; set; }
        [Display(Name = "Seçenek 3")]
        [Required]
        public string choiceC { get; set; }
        [Display(Name = "Seçenek 4")]
        [Required]
        public string choiceD { get; set; }
        [Display(Name = "Doğru Cevap")]
        [Required]
        public string answer { get; set; }
        [Display(Name = "Kategori")]
        [Required]
        public string category { get; set; }
        [Display(Name = "Seviye")]
        [Required]
        public int level { get; set; }
        [Display(Name = "Puan")]
        [Required]
        public int points { get; set; }
        [Display(Name = "Dil")]
        [Required]
        public string language { get; set; }
        [Display(Name = "Zaman")]
        [Required]
        public int time { get; set; }

        [Required]
        [Display(Name = "Durum")]
        public string status { get; set; }

        public Question()
        {
            this.status = "aktif";
        }
    }
}
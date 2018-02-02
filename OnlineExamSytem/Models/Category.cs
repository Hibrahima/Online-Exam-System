using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineExamSytem.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name="Kategori Ismi")]
        public string name { get; set; }

    }
}
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineExamSytem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineExamSytem.DAL
{
    public class ESOContext: DbContext
    {
        public ESOContext()
            : base("ESO")
        {

        }
        public DbSet<Question> Questions { get; set;}
        public DbSet<Category> Categories { get; set; }
        public DbSet<Exam> Exams { get; set; }
    }
}
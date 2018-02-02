using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineExamSytem.Models
{
    public class Exam
    {
        [Key]
        public int ExamID { get; set; }
        public string Identifier { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public int Score { get; set; }

        public string Owner { get; set; }
        public int QuestionNumber { get; set; }
        public int TrueQuestionNumber { get; set; }
        public int FalseQuestionNumber { get; set; }
        public int ExamTime { get; set; }
        public virtual ICollection<Question> ListQuestion { get; set; }
    }
}
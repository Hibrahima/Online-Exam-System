using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineExamSytem.DAL;
using OnlineExamSytem.Models;
using Microsoft.AspNet.Identity;

namespace OnlineExamSytem.Controllers
{
    public class ExamController : BaseController
    {
        private ESOContext db = new ESOContext();
        private ApplicationDbContext IdentityDb = new ApplicationDbContext();
        private List<int> list;
        private static int currrentQuestion = 0;
       
        public ExamController()
        {
            this.list = new List<int>();

        }

        public ActionResult DoQuiz()
        {
            List<string> categoryNames = new List<string>();
            foreach (Category c in db.Categories.ToList())
                categoryNames.Add(c.name);

            List<int> levels = new List<int>();
            levels.Add(1); levels.Add(2); levels.Add(3);

            List<string> lang = new List<string>();
            lang.Add("Tr"); lang.Add("Fr"); lang.Add("En");

            List<int> questNumber = new List<int>();
            questNumber.Add(1);

            ViewBag.categoryNames = new SelectList(categoryNames);
            ViewBag.quizLevels = new SelectList(levels);
            ViewBag.langNames = new SelectList(lang);
            ViewBag.quizQuestNumber = new SelectList(questNumber);
            return View();
        }

        private List<Question> generate(string categoryNames, string quizLevels, string langNames, int numOfQuestion)
        {
            Random rand = new Random();
            List<int> result = new List<int>();
            HashSet<int> check = new HashSet<int>();
            List<Question> listQuestion = GetQuestCount(categoryNames, quizLevels, langNames);
            for (int i = 0; i < numOfQuestion; i++)
            {
                int curValue = rand.Next(1, listQuestion.Count());
                while (check.Contains(curValue))
                {
                    curValue = rand.Next(1, listQuestion.Count());
                }
                result.Add(curValue);
                check.Add(curValue);
            }

            List<Question> generatedList = new List<Question>();
            for (int i = 0; i < result.Count(); i++)
            {
                generatedList.Add(listQuestion[result[i]]);
            }
            return generatedList;
        }

        [HttpGet]
        public ActionResult StartQuiz(string categoryNames, string quizLevels, string langNames, string quizQuestNumber)
        {
            currrentQuestion = 0;
           
            List<Question> listQuestion = generate(categoryNames, quizLevels, langNames, Int32.Parse(quizQuestNumber));
            Session["list"] = listQuestion;
            Question quest = listQuestion[currrentQuestion];
            return Json(quest, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SendData(string categoryNames, string quizLevels, string langNames, string quizQuestNumber)
        {
            currrentQuestion = 0;
            List<Question> listQuestion = generate(categoryNames, quizLevels, langNames, Int32.Parse(quizQuestNumber));
            Session["list"] = listQuestion;
            Question quest = listQuestion[currrentQuestion];
            return Json(listQuestion, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public String Terminate(string score, string questNumber, string trueQuestionNumber, string falseQuestionNumber)
        {
            Exam exam = new Exam();
            string userId = User.Identity.GetUserId();
            ApplicationUser user = IdentityDb.Users.Find(userId);
            exam.Identifier = user.UserName.Substring(0, 2) + user.DogumGunu.ToString().Substring(0, 2);
            exam.Date = DateTime.Now;
            exam.Score = Int32.Parse(score);
            exam.Owner = user.UserName;
            exam.QuestionNumber = Int32.Parse(questNumber);
            exam.TrueQuestionNumber = Int32.Parse(trueQuestionNumber);
            exam.FalseQuestionNumber = Int32.Parse(falseQuestionNumber);
            exam.ExamTime = GetExamTime();
            List<Question> list = (List<Question>)Session["list"];
            for (int i = 0; i < list.Count(); i++)
            {
               db.Questions.Remove(db.Questions.Find(list[i].ID));
           }
            db.SaveChanges();
            exam.ListQuestion = list;
            user.userExams.Add(exam);
          //  user.userExams = user.userExams;
            IdentityDb.Entry(user).State = EntityState.Modified;
            db.Entry(exam).State = EntityState.Modified;
            IdentityDb.SaveChanges();
            Session["exam"] = exam;

            return "Exam/ShowExamDetails";

        }

        [HttpGet]
        public ActionResult ShowExamDetails(int? id)
        {
            Exam exam;
            if(id==null)
                exam =(Exam)Session["exam"];
            else
            {
                exam = db.Exams.Find(id);
            }

            return View(exam);
        }

        [HttpGet]
        public ActionResult NextQuestion(string categoryNames, string quizLevels, string langNames, string quizQuestNumber)
        {
            List<Question> list = generate(categoryNames, quizLevels, langNames, Int32.Parse(quizQuestNumber));
            if (currrentQuestion < list.Count() - 1)
                currrentQuestion++;
           Question quest = list[currrentQuestion];
           
            return Json(quest, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public int GetExamTime()
        {
            List<Question> list = (List<Question>)Session["list"];
            int time=0;
            for(int i=0; i<list.Count(); i++)
                time+= list[i].time;
            return time;

        }

        

        private List<Question> GetQuestCount(string categoryNames, string quizLevels, string langNames)
        {
           
            List<Question> listQuestion = new List<Question>();
            foreach (Question q in db.Questions.ToList())
            {
                if (q.category.Equals(categoryNames, StringComparison.CurrentCultureIgnoreCase) &&
                    q.level == Int32.Parse(quizLevels) && q.language.Equals(langNames, StringComparison.CurrentCultureIgnoreCase) 
                    && q.status.Equals("aktif", StringComparison.CurrentCultureIgnoreCase))
                {
                    listQuestion.Add(q);
                }
            }
            return listQuestion;
        }

        [HttpGet]
        public ActionResult GetJsonData(string categoryNames, string quizLevels, string langNames)
        {
            List<Question> list = GetQuestCount(categoryNames, quizLevels, langNames);
            List<int> listOfIndices = new List<int>();
            for (int i = 1; i <= list.Count(); i++)
               listOfIndices.Add(i);

            return Json(listOfIndices, JsonRequestBehavior.AllowGet);

        }
        // GET: Exam
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = IdentityDb.Users.Find(userId);
            if (user.rol.Equals("user", StringComparison.CurrentCultureIgnoreCase))
                return View(user.userExams.ToList());
            else
                return View(db.Exams.ToList());
        }

        // GET: Exam/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = db.Exams.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        // GET: Exam/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Exam/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExamID,Identifier,Date,Score,Owner,QuestionNumber,TrueQuestionNumber,FalseQuestionNumber,ID")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                db.Exams.Add(exam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(exam);
        }

        // GET: Exam/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = db.Exams.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        // POST: Exam/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExamID,Identifier,Date,Score,Owner,QuestionNumber,TrueQuestionNumber,FalseQuestionNumber,ID")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(exam);
        }

        // GET: Exam/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = db.Exams.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        // POST: Exam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Exam exam = db.Exams.Find(id);
            db.Exams.Remove(exam);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

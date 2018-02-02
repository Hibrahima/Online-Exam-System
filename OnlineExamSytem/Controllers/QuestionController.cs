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

namespace OnlineExamSytem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuestionController : BaseController
    {
        private ESOContext db = new ESOContext();

        // GET: Question
        public ActionResult GetAllQuestions()
        {
            return View(db.Questions.ToList());
        }

        // GET: Question/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: Question/Create
        public ActionResult Create()
        {
            populateDropDowns();  
            return View();
        }

        private void populateDropDowns()
        {
            List<int> levels = new List<int>();
            levels.Add(1); levels.Add(2); levels.Add(3);
            List<string> lang = new List<string>();
            lang.Add("Tr"); lang.Add("Fr"); lang.Add("En");
            List<string> status = new List<string>();
            status.Add("Aktif"); status.Add("Inaktif");


            ViewBag.category = new SelectList(db.Categories.ToList(), "Name", "Name");
            ViewBag.level = new SelectList(levels);
            ViewBag.languages = new SelectList(lang);
            ViewBag.status = new SelectList(status);
        }

        // POST: Question/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question question)
        {
            if (ModelState.IsValid)
            {
                
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("GetAllQuestions");
            }

            populateDropDowns();
            return View(question);
        }

        // GET: Question/Edit/5
        public ActionResult Edit(int? id)
        {
            populateDropDowns();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Question/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetAllQuestions");
            }
            populateDropDowns();
            return View(question);
        }

        // GET: Question/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("GetAllQuestions");
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

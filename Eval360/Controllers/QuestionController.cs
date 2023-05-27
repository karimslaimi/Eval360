using Eval360.Data;
using Eval360.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Eval360.Controllers
{
    public class QuestionController : Controller
    {
        ApplicationDbContext db;

        public QuestionController(ApplicationDbContext db)
        {
            this.db = db;
        }



        // GET: QuestionController
        public ActionResult Index()
        {
            return View(this.db.Question.Include(x=>x.axeEval).ToArray());
        }


        // GET: QuestionController/Create
        public ActionResult Create()
        {
            ViewBag.axeList = new SelectList(this.db.AxeEval.ToArray(), "id", "name");

            return View(new Question());
        }

        // POST: QuestionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question question)
        {
            if (question.axeEval != null && question.axeEval.id != 0)
            {
                ModelState.Remove(nameof(question.axeEval.name));
                ModelState.Remove("axeEval.name");
            }
            if (ModelState.IsValid)
            {
                var axe = this.db.AxeEval.FirstOrDefault(x => x.id == question.axeEval.id);
                question.axeEval = axe;
                question.isEnabled = true;
                db.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.axeList = new SelectList(this.db.AxeEval.ToArray(), "id", "name");
            return View(question);
        }

        // GET: QuestionController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            ViewBag.axeList = new SelectList(this.db.AxeEval.ToArray(), "id", "name");

            return View(this.db.Question.Find(id));
        }

        // POST: QuestionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Question question)
        {
            if (question.axeEval != null && question.axeEval.id != 0)
            {
                ModelState.Remove(nameof(question.axeEval.name));
                ModelState.Remove("axeEval.name");
            }

            if (ModelState.IsValid)
            {
                var axe = this.db.AxeEval.FirstOrDefault(x => x.id == question.axeEval.id);
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.axeList = new SelectList(this.db.AxeEval.ToArray(), "id", "name");
            return View(question);
        }

        // GET: QuestionController/Delete/5
        public ActionResult Delete(int id)
        {
            var question = db.Question.Find(id);
            if (question == null)
            {
                return NotFound();
            }
            this.db.Question.Remove(question);
            this.db.SaveChanges();
            return RedirectToAction("Index");
        }

       
    }
}

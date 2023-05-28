using Eval360.Data;
using Eval360.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Eval360.Controllers
{
    public class CompagnieController : Controller
    {
        ApplicationDbContext db;
        private UserManager<User> userManager;

        public CompagnieController(ApplicationDbContext db, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.db = db;
        }





        // GET: CompagnieController
        public ActionResult Index()
        {
            return View(this.db.Compagnie.Include(x=>x.employee).ToArray());
        }

        // GET: CompagnieController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompagnieController/Create
        public ActionResult Create()
        {
            var users = this.userManager.GetUsersInRoleAsync("Employee").Result.ToArray();
            ViewBag.employeeList = new SelectList(users.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");
            ViewBag.questionList = this.db.AxeEval.Include(x => x.questions).ToArray();
            return View(new Compagnie());
        }

        // POST: CompagnieController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Compagnie compagnie, IFormCollection collection)
        {
            string questionsValue = collection["question"];

            // Split the field value
            string[] questionsList = questionsValue.Split(',');

            var questions = this.db.Question.Where(x => questionsList.Contains(x.id.ToString()));
            if (ModelState.IsValid)
            {

            }
            var users = this.userManager.GetUsersInRoleAsync("Employee").Result.ToArray();
            ViewBag.employeeList = new SelectList(users.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");
            ViewBag.questionList = this.db.AxeEval.Include(x => x.questions).ToArray();
            return View(compagnie);
        }

        // GET: CompagnieController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompagnieController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompagnieController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompagnieController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

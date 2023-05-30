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
            var compagnies = this.db.Compagnie.Include(x => x.employee).Include(x=>x.compagnieQuestions).Include(x=>x.compagnieUser).ToArray();
            return View(compagnies) ;
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
            string questionsValue = collection["questions"];
            string usersValue = collection["users"];

            // Split the field value
            string[] questionsList = questionsValue.Split(',');
            string[] usersList = usersValue.Split(',');

            var questions = this.db.Question.Where(x => questionsList.Contains(x.id.ToString())).ToArray();
            var users = this.db.Users.Where(x => usersList.Contains(x.Id)).ToList();
            ModelState.Remove("compagnieUser");
            ModelState.Remove("compagnieQuestions");
            ModelState.Remove("compagnieReponses");
            ModelState.Remove("employee.compagnieReponses");
            ModelState.Remove("employee.compagnieUser");
            ModelState.Remove("employee.compagnies");


            if (ModelState.IsValid)
            {
                compagnie.employee = this.userManager.FindByIdAsync(compagnie.employee.Id).Result;
                this.db.Compagnie.Add(compagnie);
                this.db.SaveChanges();
                List<CompagnieQuestion> compagnieQuestions = new();
                foreach (var question in questions)
                {
                    CompagnieQuestion compagnieQuestion = new CompagnieQuestion();
                    compagnieQuestion.compagnie = compagnie;
                    compagnieQuestion.question = question;
                    compagnieQuestions.Add(compagnieQuestion);
                }
                this.db.CompagnieQuestions.AddRange(compagnieQuestions);

                List<CompagnieUser> compagnieUsers = new();
                foreach (var user in users)
                {
                    CompagnieUser compagnieUser = new CompagnieUser();
                    compagnieUser.compagnie = compagnie;
                    compagnieUser.user = user;
                    compagnieUsers.Add(compagnieUser);
                }
                this.db.CompagnieUser.AddRange(compagnieUsers);
                this.db.SaveChanges();
                return RedirectToAction("index");
            }

            var usersVB = this.userManager.GetUsersInRoleAsync("Employee").Result.ToArray();
            ViewBag.employeeList = new SelectList(users.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");
            ViewBag.questionList = this.db.AxeEval.Include(x => x.questions).ToArray();
            return View(compagnie);
        }

        // GET: CompagnieController/Edit/5
        public ActionResult Edit(int id)
        {
            var users = this.userManager.GetUsersInRoleAsync("Employee").Result.ToArray();
            ViewBag.employeeList = new SelectList(users.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");
            ViewBag.questionList = this.db.AxeEval.Include(x => x.questions).ToArray();
            var compagnie = this.db.Compagnie.Where(x => x.id == id).Include(x => x.compagnieUser).Include(x=>x.compagnieQuestions).FirstOrDefault();
            return View(compagnie);
        }

        // POST: CompagnieController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            //check if the users have changed, the questions too
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

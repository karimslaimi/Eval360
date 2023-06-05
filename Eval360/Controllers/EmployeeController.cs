using Eval360.Data;
using Eval360.Models;
using Eval360.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Eval360.Controllers
{
    [CustomAuthorization(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<User> userManager;


        public EmployeeController(ApplicationDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;


        }

        public IActionResult Index()
        {
            return View();
        }



        public IActionResult myEval()
        {
            var currentUser = this.userManager.FindByNameAsync(User.Identity.Name).Result;
            var evals = this.db.Compagnie.Where(x => x.userId == currentUser.Id).Include(x => x.compagnieQuestions).ToList();
            return View(evals);
        }

        public IActionResult toEval()
        {
            var currentUser = this.userManager.FindByNameAsync(User.Identity.Name).Result;

            var evals = this.db.Compagnie.Where(compagnie =>compagnie.dateDebut<= DateTime.Now && compagnie.dateFin>= DateTime.Now 
            && compagnie.compagnieUser.Any(u => u.userId == currentUser.Id) && !compagnie.compagnieQuestions.Any(q=>q.reponses.Any(u=>u.userId == currentUser.Id)))
                .Include(e=>e.employee).Include(x=>x.compagnieQuestions).ThenInclude(r=>r.reponses).ToArray();

            return View(evals);
        }

        public IActionResult historique()
        {
            var currentUser = this.userManager.FindByNameAsync(User.Identity.Name).Result;

            var evals = this.db.Compagnie.Where(x=> x.compagnieQuestions.Any(s=>s.reponses.Any(s => s.userId == currentUser.Id)))
                .Include(x => x.compagnieQuestions).ThenInclude(r => r.reponses).ToArray();

            return View(evals);
        }


        public IActionResult doEval(int id)
        {
            var users = this.userManager.GetUsersInRoleAsync("Employee").Result.ToArray();
            ViewBag.employeeList = new SelectList(users.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");
            var compagnie = this.db.Compagnie.Find(id);
            var compagnieQuestions = this.db.CompagnieQuestions.Where(x => x.compagnieId == id).Include(q=>q.question).ThenInclude(a=>a.axeEval).ToArray();
            ViewBag.axe = this.db.AxeEval.ToArray();
            ViewBag.compagnieQuestion = compagnieQuestions;
            return View(compagnie);
        }

        [HttpPost]
        public IActionResult doEval(Compagnie compagnie, IFormCollection collection)
        {
            var currentUser = this.userManager.FindByNameAsync(User.Identity.Name).Result;
            var compagnieQuestions = this.db.CompagnieQuestions.Where(x => x.compagnieId == compagnie.id).Include(x => x.question).ToArray();
            List<CompagnieReponse> reponses = new();
            foreach(CompagnieQuestion compagnieQuestion in compagnieQuestions)
            {
                var compagnieResponse = new CompagnieReponse();
                compagnieResponse.compagnieQuestionId = compagnieQuestion.id;
                compagnieResponse.userId = currentUser.Id;
                compagnieResponse.note = int.Parse(collection[(compagnieQuestion.questionId).ToString()]);
                reponses.Add(compagnieResponse);
            }

            this.db.CompagnieResponse.AddRange(reponses);
            this.db.SaveChanges();

            return RedirectToAction("toEval");

        }

    }
}

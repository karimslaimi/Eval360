using Eval360.Data;
using Eval360.Models;
using Eval360.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
            string id = this.userManager.FindByNameAsync(User.Identity.Name).Result.Id;
        
            ViewBag.axeList = this.db.AxeEval.ToArray();
            ViewBag.compagnieCount = this.db.Compagnie.Where(c => c.userId == id).Count();
            ViewBag.responseCount = this.db.CompagnieResponse.Where(c => c.CompagnieQuestion.compagnie.userId == id).GroupBy(x => new { x.userId, x.CompagnieQuestion.compagnieId }).Count();
            ViewBag.latestCompagnies = this.db.Compagnie.Where(c => c.userId == id).Include(x => x.employee).OrderByDescending(x => x.dateDebut).Take(6);
            ViewBag.responseByAxe = this.db.AxeEval.Select(ae => new
            {
                AxeEvalName = ae.name,
                AverageResponse = ae.questions
                                    .SelectMany(q => q.compagnieQuestions).Where(qc => qc.compagnie.userId == id)
                                    .SelectMany(cq => cq.reponses)
                                    .Average(r => (double?)r.note)
            }).OrderBy(a => a.AxeEvalName).Select(x => x.AverageResponse);

            ViewBag.reponseByEvaluateur = JsonConvert.SerializeObject(getResponseByQualite(id));
            ViewBag.compagnieCountByMonth = JsonConvert.SerializeObject(this.getCompagnieByMonthChart(id));

            return View();

        }

        private dynamic getCompagnieByMonthChart(string id)
        {

            var companiesPerMonth = new List<int>();
            for (int month = 1; month <= 12; month++)
            {
                var companiesCreated = this.db.Compagnie
                    .Where(c => c.dateDebut.Month == month && c.userId == id);
                companiesPerMonth.Add(companiesCreated.Count());
            }




            return companiesPerMonth;
        }


        private dynamic getResponseByQualite(string id)
        {
            Dictionary<string, double> data = new Dictionary<string, double>() { { "Autoévaluation", 0.0 }, { "Collaborateur", 0.0 }, { "Collègue", 0.0 }, { "Hiérarchie", 0.0 } };

            foreach (string key in data.Keys)
            {
                if (this.db.Compagnie.Where(c => c.qualiteEvaluateur.Equals(key) && c.userId == id).Count() != 0
                    && this.db.Compagnie.Where(c => c.qualiteEvaluateur.Equals(key)).SelectMany(s => s.compagnieQuestions).SelectMany(s => s.reponses).Count() != 0)
                {
                    data[key] = this.db.Compagnie.Where(c => c.qualiteEvaluateur.Equals(key) && c.userId == id).SelectMany(s => s.compagnieQuestions).SelectMany(s => s.reponses).Average(r => r.note);
                }
            }
            return data.Values;
        }


        public IActionResult myEval()
        {
            var currentUser = this.userManager.FindByNameAsync(User.Identity.Name).Result;
            var evals = this.db.Compagnie.Where(x => x.userId == currentUser.Id && x.dateDebut <= DateTime.Now).Include(x => x.compagnieQuestions).ThenInclude(r => r.reponses).Include(x => x.compagnieUser).ThenInclude(x => x.user).ToList();
            return View(evals);
        }

        public IActionResult toEval()
        {
            var currentUser = this.userManager.FindByNameAsync(User.Identity.Name).Result;

            var evals = this.db.Compagnie.Where(compagnie => compagnie.dateDebut <= DateTime.Now && compagnie.dateFin >= DateTime.Now
            && compagnie.compagnieUser.Any(u => u.userId == currentUser.Id) && !compagnie.compagnieQuestions.Any(q => q.reponses.Any(u => u.userId == currentUser.Id)))
                .Include(e => e.employee).Include(x => x.compagnieQuestions).ThenInclude(r => r.reponses).ToArray();

            return View(evals);
        }

        public IActionResult historique()
        {
            var currentUser = this.userManager.FindByNameAsync(User.Identity.Name).Result;

            var evals = this.db.Compagnie.Where(x => x.compagnieQuestions.Any(s => s.reponses.Any(s => s.userId == currentUser.Id)))
                .Include(x => x.compagnieQuestions).ThenInclude(r => r.reponses).Include(x => x.employee).ToArray();
            ViewBag.UserId = currentUser.Id;
            return View(evals);
        }


        public IActionResult doEval(int id)
        {
            var users = this.userManager.GetUsersInRoleAsync("Employee").Result.ToArray();
            ViewBag.employeeList = new SelectList(users.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");
            var compagnie = this.db.Compagnie.Find(id);
            var compagnieQuestions = this.db.CompagnieQuestions.Where(x => x.compagnieId == id).Include(q => q.question).ThenInclude(a => a.axeEval).ToArray();
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
            foreach (CompagnieQuestion compagnieQuestion in compagnieQuestions)
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

        public IActionResult consultEval(int id)
        {
            var currentUser = this.userManager.FindByNameAsync(User.Identity.Name).Result;
            var responses = this.db.CompagnieResponse
                .Where(r => r.CompagnieQuestion.compagnie.id == id && r.CompagnieQuestion.compagnie.userId== currentUser.Id)
                .GroupBy(r => new
                {
                    userId = r.userId,
                    name = r.user.preNom + " " + r.user.Nom,
                    compagnie = r.CompagnieQuestion.compagnie.title,
                    compagnieId = r.CompagnieQuestion.compagnieId
                })
                .Select(s => new
                {
                    compagnie = s.Key.compagnie,
                    user = s.Key.name,
                    note = s.Average(n => n.note),
                    userId = s.Key.userId,
                    compagnieId = s.Key.compagnieId
                })
                .ToList();
            ViewBag.reponses = responses;
            return View();

        }

        public IActionResult consultMyEvals(int id)
        {
            var reponses = this.db.CompagnieResponse.Where(x => x.CompagnieQuestion.compagnie.dateFin > DateTime.Now
            && x.CompagnieQuestion.compagnie.employee.UserName == User.Identity.Name).Include(x => x.CompagnieQuestion.compagnie).ToArray();

            return View(reponses);
        }

        public IActionResult consultReponse(string userId, int compagnieId)
        {
            var users = this.userManager.GetUsersInRoleAsync("Employee").Result.ToArray();
            ViewBag.employeeList = new SelectList(users.Select(x => new { Id = x.Id, libelle = x.Nom + " " + x.preNom }).ToArray(), "Id", "libelle");
            var compagnie = this.db.Compagnie.Find(compagnieId);
            var compagnieQuestions = this.db.CompagnieQuestions.Where(x => x.compagnieId == compagnieId).Include(q => q.question).ThenInclude(a => a.axeEval).Include(x => x.reponses).ToArray();
            ViewBag.axe = this.db.AxeEval.ToArray();
            ViewBag.responses = this.db.CompagnieResponse.Where(x => x.CompagnieQuestion.compagnieId == compagnieId && x.userId == userId);
            ViewBag.compagnieQuestion = compagnieQuestions;
            var user = this.userManager.FindByIdAsync(userId).Result;
            ViewBag.name = user.preNom + " " + user.Nom;
            return View(compagnie);
        }

    }
}

using Eval360.Data;
using Eval360.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Eval360.Controllers
{
    public class GestionnaitreController : Controller
    {
        ApplicationDbContext db;
        private UserManager<User> userManager;

        public GestionnaitreController(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            this.db = dbContext;
            this.userManager = userManager;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CompagnieList()
        {
            var compagnies = this.db.Compagnie.Include(x => x.employee).Include(x => x.compagnieQuestions).ThenInclude(x => x.reponses).ThenInclude(u => u.user).Include(x => x.compagnieUser).ToArray();
            return View(compagnies);
        }

        public IActionResult responseList(int id)
        {
            var responses = this.db.CompagnieResponse.Where(r => r.CompagnieQuestion.compagnie.id == id).GroupBy(r =>
            new
            {
                userId = r.userId,
                name = r.user.preNom + " " + r.user.Nom,
                compagnie = r.CompagnieQuestion.compagnie.title,
                compagnieId = r.CompagnieQuestion.compagnieId
            }).Select(s => new
            {
                compagnie = s.Key.compagnie,
                user = s.Key.name,
                note = s.Average(n => n.note),
                userId = s.Key.userId,
                compagnieId = s.Key.compagnieId
            }).ToList();

            ViewBag.reponses = responses;
            return View();
        }


        public IActionResult consultResponse(string userId, int compagnieId)
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

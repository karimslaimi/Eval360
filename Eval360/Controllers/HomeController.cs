using Eval360.Data;
using Eval360.Models;
using Eval360.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Eval360.Controllers
{
    [CustomAuthorization(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<User> userManager;
        private ApplicationDbContext db;
        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, ApplicationDbContext dbContext)
        {
            this.db = dbContext;
            this.userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.userCount = this.userManager.GetUsersInRoleAsync("Employee").Result.Count();
            ViewBag.compagnieCount = this.db.Compagnie.Count();
            ViewBag.responseCount = this.db.CompagnieResponse.GroupBy(x => new { x.userId, x.CompagnieQuestion.compagnieId }).Count();
            ViewBag.latestCompagnies = this.db.Compagnie.Include(x => x.employee).OrderByDescending(x => x.dateDebut).Take(6);
            ViewBag.axeList = this.db.AxeEval.ToArray();
            ViewBag.responseByAxe = this.db.AxeEval.Select(ae => new
            {
                AxeEvalName = ae.name,
                AverageResponse = ae.questions
                                    .SelectMany(q => q.compagnieQuestions)
                                    .SelectMany(cq => cq.reponses)
                                    .Average(r => r.note)
            }).OrderBy(a => a.AxeEvalName).Select(x=>x.AverageResponse);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using Eval360.Data;
using Eval360.Models;
using Eval360.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Dynamic;

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
            var directions = this.db.Directions.OrderBy(x => x.id).ToList();
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
                                    .Average(r => (double?)r.note)
            }).OrderBy(a => a.AxeEvalName).Select(x => x.AverageResponse);


            var res = this.db.Directions.Select(d => new
            {
                name = d.name,
                value = this.db.Compagnie.Where(c => c.employee.Poste.IdDirection == d.id).Count()
            }).ToList();
            ViewBag.compagnieByDirection = JsonConvert.SerializeObject(res);
            ViewBag.compagnieCountByMonth = JsonConvert.SerializeObject(this.getCompagnieByMonthChart(directions));
            ViewBag.DirectionList = JsonConvert.SerializeObject(directions.Select(x=>x.name).ToList());
            ViewBag.responseByAxeAndDirections = JsonConvert.SerializeObject(this.getAxeValueByDirection(directions));
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


        private dynamic getAxeValueByDirection(List<Direction> directions)
        {
            List<dynamic> data = new List<dynamic>();

            var axeEval = this.db.AxeEval.OrderBy(x => x.name).ToArray();

            foreach (var axe in axeEval)
            {
                var vals = new List<double>(directions.Count());
                foreach (var direction in directions)
                {
                    


                    var average = this.db.CompagnieResponse.Where(x => x.CompagnieQuestion.question.idAxe == axe.id
                    && x.CompagnieQuestion.compagnie.employee.Poste.IdDirection == direction.id);
                    
                   vals.Add(average.Count()!=0?average.Average(x => x.note):0);
                }
                data.Add(new { name = axe.name, data = vals });


            }
            return data;

        }

        private dynamic getCompagnieByMonthChart(List<Direction> directions)
        {

            // Create a dictionary to store the data
            List<Object> data = new List<object>();

            // Iterate over the directions and get the number of companies created in each month
            foreach (var direction in directions)
            {
                var companiesPerMonth = new List<int>();
                for (int month = 1; month <= 12; month++)
                {
                    var companiesCreated = this.db.Compagnie
                        .Where(c => c.dateDebut.Month == month && c.employee.Poste.IdDirection == direction.id);
                    companiesPerMonth.Add(companiesCreated.Count());
                }
                data.Add(new { name = direction.name, data = companiesPerMonth });
            }


            return data;
        }

    }
}
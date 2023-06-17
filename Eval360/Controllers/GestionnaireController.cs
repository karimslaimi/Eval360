using Eval360.Data;
using Eval360.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Eval360.Controllers
{
    public class GestionnaireController : Controller
    {
        ApplicationDbContext db;
        private UserManager<User> userManager;

        public GestionnaireController(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            this.db = dbContext;
            this.userManager = userManager;

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


            ViewBag.reponseByEvaluateur = JsonConvert.SerializeObject(getResponseByQualite());

            var res = this.db.Directions.Select(d => new
            {
                name = d.name,
                value = this.db.Compagnie.Where(c => c.employee.Poste.IdDirection == d.id).Count()
            }).ToList();
            ViewBag.compagnieByDirection = JsonConvert.SerializeObject(res);
            ViewBag.compagnieCountByMonth = JsonConvert.SerializeObject(this.getCompagnieByMonthChart(directions));
            ViewBag.DirectionList = JsonConvert.SerializeObject(directions.Select(x => x.name).ToList());
            ViewBag.responseByAxeAndDirections = JsonConvert.SerializeObject(this.getAxeValueByDirection(directions));
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

                    vals.Add(average.Count() != 0 ? average.Average(x => x.note) : 0);
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


        private dynamic getResponseByQualite()
        {
            Dictionary<string, double> data = new Dictionary<string, double>() { { "Autoévaluation", 0.0 }, { "Collaborateur", 0.0 }, { "Collègue", 0.0 }, { "Hiérarchie", 0.0 } };

            foreach (string key in data.Keys)
            {
                if (this.db.Compagnie.Where(c => c.qualiteEvaluateur.Equals(key)).Count() != 0
                    && this.db.Compagnie.Where(c => c.qualiteEvaluateur.Equals(key)).SelectMany(s => s.compagnieQuestions).SelectMany(s => s.reponses).Count() != 0)
                {
                    data[key] = this.db.Compagnie.Where(c => c.qualiteEvaluateur.Equals(key)).SelectMany(s => s.compagnieQuestions).SelectMany(s => s.reponses).Average(r => r.note);
                }
            }
            return data.Values;
        }

    }
}

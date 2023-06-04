using Eval360.Data;
using Eval360.Models;
using Eval360.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

            var evals = this.db.Compagnie.Where(compagnie => compagnie.compagnieUser.Any(u => u.userId == currentUser.Id))
                .Include(e=>e.employee).Include(x=>x.compagnieQuestions).ThenInclude(r=>r.reponses).ToArray();

            /*from compagnie in this.db.Compagnie
                    join compagnieUser in this.db.CompagnieUser on compagnie.id equals compagnieUser.idCompagnie
                    where compagnieUser.userId == currentUser.Id && compagnie.dateFin >= DateTime.Now
                    select new { compagnie, compagnie.compagnieQuestions };*/
            return View(evals);
        }

        public IActionResult historique()
        {
            var currentUser = this.userManager.FindByNameAsync(User.Identity.Name).Result;

            var evals = from compagnie in this.db.Compagnie
                        join compagnieQuestion in this.db.CompagnieQuestions on compagnie.id equals compagnieQuestion.compagnieId
                        join compagnieResponse in this.db.CompagnieResponse on compagnieQuestion.id equals compagnieResponse.compagnieQuestionId
                        where compagnieResponse.userId == currentUser.Id
                        select compagnie;

            return View(evals);
        }


    }
}
